using ECOMM.Business.Abstract;
using ECOMM.Core.Enums;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly IUserService _userService;
    private readonly ISessionService _sessionService;
    private readonly UserManager<User> _userManager;
    public OrderController(IUserService userService, IProductService productService, IOrderService orderService, ISessionService sessionService, UserManager<User> userManager)
    {
        _userService = userService;
        _productService = productService;
        _orderService = orderService;
        _sessionService = sessionService;
        _userManager = userManager;
    }

    // Ödeme Sayfası
    public async Task<IActionResult> Payment()
    {
        var cartItems = _sessionService.GetCartItems();
        var totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

        // Get logged-in user via UserManager
        var user = await _userManager.GetUserAsync(User);  // 'User' comes from the controller context

        var paymentViewModel = new PaymentViewModel
        {
            CartItems = cartItems,
            TotalAmount = totalAmount,
            UserName = user?.UserName,
            Email = user?.Email,
            ShippingCity = user?.ShippingCity
        };

        return View(paymentViewModel);
    }


    // Ödeme İşlemi
    [HttpPost]
    public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetByIdAsync(model.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View("Payment", model);
            }

            // Convert string PaymentMethod to PaymentMethodEnum
            if (!Enum.TryParse(model.PaymentMethod, out PaymentMethodEnum paymentMethod))
            {
                ModelState.AddModelError("", "Geçersiz ödeme yöntemi.");
                return View("Payment", model);
            }

            var order = new Orders
            {
                UserId = model.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                Status = OrderStatus.Pending,
                PaymentInfo = new PaymentInfo
                {
                    PaymentMethod = paymentMethod,
                    PaymentDate = DateTime.Now,
                    PaymentStatus = "Successful"
                },
                ShippingInfo = new ShippingInfo
                {
                    Address = model.ShippingAddress,
                    City = model.ShippingCity,
                    PostalCode = model.ShippingPostalCode
                },
                OrderItems = model.CartItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            await _orderService.CreateAsync(order);
            _sessionService.ClearCart();

            return RedirectToAction("Success");
        }

        return View("Payment", model);
    }


    public IActionResult Success()
    {
        return View();
    }

    public async Task<IActionResult> OrderHistory(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return View(orders);
    }
}
