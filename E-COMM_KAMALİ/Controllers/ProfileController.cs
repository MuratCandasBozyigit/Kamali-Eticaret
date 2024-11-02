using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

  

        public ProfileController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
