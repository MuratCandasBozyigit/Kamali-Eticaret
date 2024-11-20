using Microsoft.AspNetCore.Mvc;
using ECOMM.Business.Abstract;
using ECOMM.Core.ViewModels;
using System.Linq;

namespace ECOMM.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ISessionService _sessionService;

        public CheckoutController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Sepet öğelerini ve toplam tutarı al
            var cartItems = _sessionService.GetCartItems();
            var totalAmount = cartItems.Sum(c => c.TotalPrice); // Toplam tutar hesapla

            // Checkout özetini doldur
            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalAmount = totalAmount
            };

            return View(viewModel); // Sepet bilgilerini kullanıcıya gönder
        }

        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Siparişin özetini orderController'a gönder
                return RedirectToAction("PlaceOrder", "Order", viewModel);
            }

            return RedirectToAction("Index", "Checkout");
        }
    }
}
