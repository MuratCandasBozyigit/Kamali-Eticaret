using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Core.ViewModels;
using ECOMM.Core.Models;
using ECOMM.Business.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMM.Web.Controllers
{
    // [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderRepository;
        private readonly ISessionService _sessionService;
        private readonly IUserService _userRepository;

        public OrderController(IOrderService orderRepository, IUserService userRepository, ISessionService sessionService)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository), "OrderRepository is null. Please ensure it is registered in the DI container.");
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository), "User Repository is null. Please ensure it is registered in the DI container.");
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService), "SessionService is null. Please ensure it is registered in the DI container.");
        }

        // GET: Checkout
        // GET: Checkout
        [HttpGet("Checkout")]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var userId = User.Identity?.Name;

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "Oturum açılmamış. Lütfen giriş yapınız.";
                    return RedirectToAction("Login", "Account");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    TempData["Error"] = "Kullanıcı bilgileri bulunamadı. Lütfen destek ekibiyle iletişime geçiniz.";
                    return RedirectToAction("Index", "ShopCart");
                }

                var cartItems = _sessionService.GetCartItems();
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["Error"] = "Sepetinizde ürün bulunmamaktadır.";
                    return RedirectToAction("Index", "ShopCart");
                }

                var viewModel = new PaymentViewModel
                {
                    UserName = user.FullName ?? "Bilinmeyen Kullanıcı",
                    Email = user.Email ?? "Email bulunamadı",
                    Phone = user.PhoneNumber ?? "Telefon numarası belirtilmemiş",
                    ShippingAddress = user.Adress ?? "Adres bulunamadı",
                    ShippingCity = user.City ?? "Şehir bilgisi eksik",
                    CartItems = cartItems.Select(item => new CartItemViewModel
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product?.ProductName ?? "Ürün adı eksik",
                        Price = item.Product?.ProductPrice ?? 0,
                        Quantity = item.Quantity,
                        ImagePath = item.Product?.ImagePath ?? "/images/default.png"
                    }).ToList(),
                    TotalAmount = cartItems.Sum(item => item.Quantity * (item.Product?.ProductPrice ?? 0)),
                    TotalPrice = cartItems.Sum(item => item.Quantity * (item.Product?.ProductPrice ?? 0))
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata oluştu: " + ex.Message;
                Console.Error.WriteLine("Checkout Error: " + ex);
                return RedirectToAction("Error", "Home");
            }
        }



        // POST: ProcessPayment
        public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Gönderilen bilgilerde eksiklikler var. Lütfen tekrar kontrol edin.";
                return View("Checkout", model);
            }

            try
            {
                var userId = User.Identity?.Name;

                if (string.IsNullOrEmpty(userId))
                {
                    TempData["Error"] = "Oturum açılmamış. Lütfen giriş yapınız.";
                    return RedirectToAction("Login", "Account");
                }

                var isPaymentSuccessful = ProcessPaymentMethod(model.PaymentMethod);
                if (!isPaymentSuccessful)
                {
                    TempData["Error"] = "Ödeme işlemi başarısız oldu. Lütfen tekrar deneyin.";
                    return View("Checkout", model);
                }

                var order = new Orders
                {
                    UserId = userId,
                    ShippingAddress = model.ShippingAddress,
                    ShippingCity = model.ShippingCity,
                    PaymentMethod = model.PaymentMethod,
                    TotalAmount = model.TotalAmount,
                    OrderDate = DateTime.Now,
                    OrderItems = model.CartItems.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };

                await _orderRepository.AddAsync(order);
                _sessionService.ClearCart();  // Sepeti temizliyoruz

                return RedirectToAction("OrderSummary", new { orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Sipariş işlemi sırasında bir hata oluştu: " + ex.Message;
                return View("Checkout", model);
            }
        }

        // GET: Order Summary
        [HttpGet]
        public async Task<IActionResult> OrderSummary(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {
                    TempData["Error"] = "Sipariş bulunamadı. Lütfen destek ekibiyle iletişime geçiniz.";
                    return RedirectToAction("Index", "Home");
                }

                var viewModel = new OrderSummaryViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    Total = order.TotalAmount,
                    ShippingAddress = order.ShippingInfo.ShippingAddress ?? "Adres eksik",
                    PaymentMethod = order.PaymentMethod ?? "Bilinmeyen Ödeme Yöntemi",
                    OrderItems = order.OrderItems.Select(item => new OrderItemViewModel
                    {
                        ProductName = item.Product?.ProductName ?? "Ürün adı eksik",
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata oluştu: " + ex.Message;
                Console.Error.WriteLine("OrderSummary Error: " + ex);
                return RedirectToAction("Index", "Home");
            }
        }

        // Yardımcı metod: Ödeme işlemini simüle et
        private bool ProcessPaymentMethod(string paymentMethod)
        {
            return !string.IsNullOrEmpty(paymentMethod) && paymentMethod != "Havale";
        }
    }
}