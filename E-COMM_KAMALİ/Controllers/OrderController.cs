using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using ECOMM.Business.Concrete;
using System.Collections.Generic;

namespace E_COMM_KAMALİ.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService; // Oturum yönetimi için

        public OrderController(IUserService userService, IProductService productService, IOrderService orderService, ISessionService sessionService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
            _sessionService = sessionService;
        }

        // Ödeme sayfasına yönlendirme
        public IActionResult Payment()
        {
            var cartItems = _sessionService.GetCartItems(); // Sepet öğelerini al
            var totalAmount = cartItems.Sum(item => item.Price * item.Quantity); // Toplam tutarı hesapla

            var paymentViewModel = new PaymentViewModel
            {
                CartItems = cartItems,
                TotalAmount = totalAmount
            };

            return View(paymentViewModel); // Ödeme sayfasına gönder
        }

        // Ödeme işlemini işleme
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı bilgilerini al
                var user = await _userService.GetByIdAsync(model.UserId);

                // Yeni sipariş oluştur
                var order = new Orders
                {
                    UserId = model.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = model.TotalAmount,
                    ShippingAddress = model.ShippingAddress,
                    Status = OrderStatus.Pending,
                    PaymentMethod = model.PaymentMethod,
                    OrderItems = model.CartItems.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };

                // Siparişi veritabanına kaydet
                await _orderService.CreateAsync(order);

                // Sepeti temizle
                _sessionService.ClearCart();

                return RedirectToAction("Success"); // Başarılı ödeme sonrası yönlendirme
            }

            return View("Payment", model); // Hata varsa tekrar ödeme sayfasını göster
        }

        // Başarılı ödeme sayfası
        public IActionResult Success()
        {
            return View(); // Başarılı ödeme sayfasını göster
        }

        // Sipariş geçmişini görüntüleme
        public async Task<IActionResult> OrderHistory(string userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId); // Kullanıcıya ait siparişleri al
            return View(orders); // Sipariş geçmişini göster
        }
    }
}