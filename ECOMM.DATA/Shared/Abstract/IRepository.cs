using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ECOMM.Data.Shared.Abstract
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<IQueryable<T>> GetAllAsync();
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<List<T>> AddRangeAsync(List<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(Guid guid);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task SaveAsync();
        Task<bool> DeleteAsync(Guid guid);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId);
    }
}
