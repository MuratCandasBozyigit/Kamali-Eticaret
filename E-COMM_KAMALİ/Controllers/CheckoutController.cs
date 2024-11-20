using Microsoft.AspNetCore.Mvc;
using ECOMM.Business.Abstract;
using ECOMM.Core.ViewModels;
using System.Linq;

namespace ECOMM.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;

        public CheckoutController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Sepet öğelerini getir ve toplam tutarı hesapla
            var cartItems = _cartService.GetCartItems();
            var totalAmount = cartItems.Sum(c => c.Price * c.Quantity);

            // Checkout özetini doldur
            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalAmount = totalAmount
            };

            return View(viewModel); // Kullanıcıdan onay almak için
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
