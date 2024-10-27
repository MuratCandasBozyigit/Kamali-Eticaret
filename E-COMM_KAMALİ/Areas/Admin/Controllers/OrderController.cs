using Microsoft.AspNetCore.Mvc;
using ECOMM.Business;
using ECOMM.Business.Abstract;
namespace E_COMM_KAMALİ.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        public OrderController(IOrderService orderService, ICategoryService categoryService, IProductService productService, IUserService userService)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _productService = productService;
            _userService = userService;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
