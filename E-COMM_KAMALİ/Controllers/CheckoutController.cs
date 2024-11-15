using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public CheckoutController(IProductService productService, IUserService userService, IOrderService orderService)
        {
            _productService = productService;
            _userService = userService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Chat-Gpt ye nasıl yapılması gerektiğini sor "bankalar nasıl istiyor neleri ypamam lazımki bankalar kabul etsin
    }
}
