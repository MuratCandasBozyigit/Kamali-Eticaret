using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Areas.Admin.Controllers
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
