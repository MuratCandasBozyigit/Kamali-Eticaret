using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetComments()
        {
            var comments = await _commentService.GetPendingCommentsAsync();
            var result = comments.Select(c => new
            {
                c.Id,
                c.Content,
                c.AuthorId,
            //    AuthorName = c.Author?.UserName ?? "Unknown",
                AuthorName = c.Author?.FullName ?? "Unknown",
                ProductName = c.Product?.ProductName ?? "Unknown",
                c.DateCommented,
                c.IsApproved
            });

            return Json(new { data = result });
        }

        public async Task<IActionResult> Approve(int id)
        {
            await _commentService.ApproveCommentAsync(id);
            return Ok();
        }

        public async Task<IActionResult> Reject(int id)
        {
            await _commentService.RejectCommentAsync(id);
            return Ok();
        }
    }
}
