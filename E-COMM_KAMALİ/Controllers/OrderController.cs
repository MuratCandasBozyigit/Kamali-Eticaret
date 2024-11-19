using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productSercice;
        private readonly IUserService _userService;

        public OrderController(IUserService userService, IProductService productSercice, IOrderService orderService)
        {
            _userService = userService;
            _productSercice = productSercice;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
