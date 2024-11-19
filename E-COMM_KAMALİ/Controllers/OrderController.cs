using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMM.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ISessionService _sessionService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, ISessionService sessionService, IProductService productService, IUserService userService)
        {
            _orderService = orderService;
            _sessionService = sessionService;
            _productService = productService;
            _userService = userService;
        }

        // Ödeme sayfası (checkout)
        public IActionResult Checkout()
        {
            var cartItems = _sessionService.GetCartItems(); // Sepetteki ürünleri al
            var totalAmount = CalculateTotalAmount(cartItems); // Toplam tutarı hesapla

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalAmount = totalAmount,
                User = _userService.GetByIdAsync(User.Identity.Name).Result // Kullanıcı bilgilerini al
            };

            return View(checkoutViewModel); // Checkout sayfasını kullanıcıya göster
        }

        // Sipariş oluşturma
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                // Sepet öğelerini al
                var cartItems = _sessionService.GetCartItems();

                // Kullanıcı bilgilerini al
                var user = await _userService.GetByIdAsync(orderViewModel.UserId);

                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return RedirectToAction("Checkout");
                }

                // Siparişi oluştur
                var order = new Orders
                {
                    UserId = user.Id,
                    OrderDate = System.DateTime.Now,
                    TotalAmount = orderViewModel.TotalAmount,
                    OrderItems = new List<ECOMM.Core.Models.OrderItem>() // Corrected type to model
                };

                // Sepet öğelerini sipariş öğelerine dönüştür
                foreach (var cartItem in cartItems)
                {
                    var product = await _productService.GetByIdAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        order.OrderItems.Add(new ECOMM.Core.Models.OrderItem // Corrected type to model
                        {
                            ProductId = cartItem.ProductId,
                            Quantity = cartItem.Quantity,
                            Price = product.ProductPrice
                        });
                    }
                }

                // Siparişi veritabanına ekle
                await _orderService.AddAsync(order);

                // Sipariş tamamlandıktan sonra sepeti temizle
                _sessionService.ClearCart();

                // Ödeme işlemine yönlendirme veya ödeme onay sayfası
                return RedirectToAction("Payment", new { orderId = order.Id });
            }

            // Hatalı model durumunda ödeme sayfasına geri dön
            return RedirectToAction("Checkout");
        }

        // Ödeme sayfası
        public IActionResult Payment(int orderId)
        {
            var order = _orderService.GetByIdAsync(orderId).Result;
            if (order == null)
            {
                return RedirectToAction("Index", "Home"); // Sipariş bulunamadıysa ana sayfaya yönlendir
            }

            var paymentViewModel = new PaymentViewModel
            {
                OrderId = order.OrderId.ToString(), // OrderId'yi string'e dönüştürmek gerekirse
                TotalAmount = order.TotalAmount
            };

            return View(paymentViewModel); // Ödeme sayfasını göster
        }

        // Ödeme işleme (işlem sonrasında sipariş durumu güncellenir)
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int orderId, string paymentMethod)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
            {
                return RedirectToAction("Index", "Home"); // Sipariş bulunamadıysa ana sayfaya yönlendir
            }

            // Burada ödeme işleme mantığını entegre etmelisiniz.
            // Ödeme başarılı ise siparişin durumunu güncelle
            order.PaymentMethod = paymentMethod;
            order.PaymentStatus = "Success"; // Ödeme başarılı ise statü güncellenir

            // Sipariş güncellemesi
            await _orderService.UpdateAsync(order);

            // Ödeme başarıyla tamamlandığında, kullanıcının sipariş detaylarını göster
            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

        // Sipariş onay sayfası
        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);
            if (order == null)
            {
                return RedirectToAction("Index", "Home"); // Sipariş bulunamadıysa ana sayfaya yönlendir
            }

            // OrderItem'ları ViewModel'e dönüştür
            var orderItems = order.OrderItems.Select(item => new ECOMM.Core.ViewModels.OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.ProductName, // Ürün adını alıyoruz
                Quantity = item.Quantity,
                Price = item.Price,
                TotalPrice = item.Quantity * item.Price // Toplam fiyatı hesaplıyoruz
            }).ToList();

            var confirmationViewModel = new OrderConfirmationViewModel
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod, // Ödeme yöntemi
                PaymentStatus = order.PaymentStatus,   // Ödeme durumu
                OrderItems = orderItems, // Dönüştürülmüş OrderItem'lar
            };

            return View(confirmationViewModel); // Sipariş onayı sayfasını göster
        }

        #region Yardımcı Metodlar

        private decimal CalculateTotalAmount(List<CartItemViewModel> cartItems)
        {
            decimal total = 0;
            foreach (var item in cartItems)
            {
                var product = _productService.GetByIdAsync(item.ProductId).Result;
                if (product != null)
                {
                    total += item.Quantity * product.ProductPrice;
                }
            }
            return total;
        }

        #endregion
    }
}
