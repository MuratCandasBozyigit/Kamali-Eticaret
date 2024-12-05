using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using Microsoft.EntityFrameworkCore;
using ECOMM.Data.Shared.Abstract;using System.Linq.Expressions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ECOMM.Business.Concrete
{
    public class CommentService : Service<Comment>, ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        public CommentService(IRepository<Comment> commentsRepository) : base(commentsRepository)
        {
            _commentRepository = commentsRepository;
        }

        public async Task<IEnumerable<Comment>> GetPendingCommentsAsync()
        {
            return await _commentRepository.GetAllAsyncQueryComment(
                c => !c.IsApproved,  // Filter for unapproved comments
                include: q => q.Include(c => c.Author).Include(c => c.Product)  // Include Author and Product
            );
        }

        public async Task<IEnumerable<Comment>> GetUserCommentsAsync(string userId)
        {
            var commentsQuery = await _commentRepository.GetAllAsyncQueryComment(
                predicate: c => c.AuthorId == userId, // Yorumları kullanıcıya göre filtrele
                include: query => query.Include(c => c.Product) // Yorumlar ile ilgili ürün bilgisini dahil et
            );

            // IQueryable üzerinde sorgu yapmamız gerektiği için ToListAsync() çağırarak veriyi alıyoruz
            var comments = await commentsQuery.ToListAsync();

            return comments;
        }




        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            return await _commentRepository.AddAsync(comment);
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            return await _commentRepository.UpdateAsync(comment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _commentRepository.DeleteAsync(id);
        }

        public async Task ApproveCommentAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment != null)
            {
                comment.IsApproved = true;
                await _commentRepository.UpdateAsync(comment);
            }
        }

        public async Task RejectCommentAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment != null)
            {
                await _commentRepository.DeleteAsync(id);
            }
        }
    }
}
