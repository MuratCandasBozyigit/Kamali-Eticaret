using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Abstract
{
    public interface ICommentService : IService<Comment>
    {
        Task<IEnumerable<Comment>> GetPendingCommentsAsync();
        Task ApproveCommentAsync(int id);
        Task RejectCommentAsync(int id);
        Task<IEnumerable<Comment>> GetUserCommentsAsync(string userId);
    }
}
