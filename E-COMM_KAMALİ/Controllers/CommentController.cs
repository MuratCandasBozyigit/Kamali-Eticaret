using ECOMM.Business.Abstract;
using ECOMM.Business.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ECOMM.Web.Areas.Admin.Controllers.ProductController;

namespace E_COMM_KAMALİ.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private IOrderService orderService;

        public CommentController(IOrderService orderService, IProductService productService, IUserService userService, ICommentService commentService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.userService = userService;
            this.commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> AddComment(Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Yorum bilgisi eksik.");
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var result = await commentService.AddAsync(comment);

            if (result == null)
            {
                return NotFound("Yorum eklenemedi.");
            }

            return Ok("Yorum başarıyla eklendi.");
        }



        public async Task<IActionResult> PendingComments()
        {
            var pendingComments = await commentService.GetPendingCommentsAsync();
            return View(pendingComments);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveComment(int id)
        {
            var comment = await commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();

            comment.IsApproved = true;
            await commentService.UpdateAsync(comment);

            TempData["Message"] = "Yorum onaylandı.";
            return RedirectToAction("PendingComments");
        }
    }

}
