using Microsoft.AspNetCore.Mvc;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMM.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Sipariş modeli oluştur
                var order = new Orders
                {
                    OrderItems = viewModel.CartItems.Select(c => new OrderItem
                    {
                        ProductId = c.ProductId,
                        ProductName = c.ProductName,
                        ImagePath = c.ImagePath,
                        Price = c.Price,
                        Quantity = c.Quantity
                    }).ToList(),
                    TotalAmount = viewModel.TotalAmount,
                    ShippingAddress = viewModel.Address,
                    PaymentMethod = viewModel.PaymentMethod,
                    OrderDate = DateTime.Now
                };

                // Siparişi kaydet
                await _orderService.PlaceOrderAsync(order);

                // Sepeti temizle
                _cartService.ClearCart();

                return RedirectToAction("Success");
            }

            return RedirectToAction("Index", "Checkout");
        }

        public IActionResult Success()
        {
            return View(); // Başarılı sipariş mesajı gösterilir
        }

        public IActionResult History()
        {
            // Kullanıcının geçmiş siparişlerini getir
            var orders = _orderService.GetOrdersByUserAsync(User.Identity.Name);
            return View(orders);
        }
    }
}
