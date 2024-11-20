using Microsoft.AspNetCore.Mvc;
using ECOMM.Core.ViewModels;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Business.Concrete;

namespace ECOMM.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ISessionService _sessionService;

        public CheckoutController(
            IOrderService orderService,
            IProductService productService,
            ICategoryService categoryService,
            ISubCategoryService subCategoryService,
            ISessionService sessionService)
        {
            _orderService = orderService;
            _productService = productService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _sessionService = sessionService;
        }

        // Display the checkout form with cart items
        [HttpGet]
        public IActionResult Index()
        {
            var cartItems = _sessionService.GetCartItems(); // Get cart items from session
            if (cartItems == null)
            {
                cartItems = new List<CartItemViewModel>(); // Initialize an empty list if cart is null
            }

            var totalAmount = cartItems.Sum(item => item.TotalPrice);

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                TotalAmount = totalAmount
            };

            return View(checkoutViewModel);
        }


        // Process the checkout (Place Order)
        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Assuming you have a valid OrderItem object in model.CartItems
                var order = new Orders
                {
                    CustomerName = model.CustomerName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Address2 = model.Address2,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    Country = model.Country,
                    PaymentMethod = model.PaymentMethod,
                    PromoCode = model.PromoCode,
                    SameAddress = model.SameAddress,
                    SaveInfo = model.SaveInfo,
                    TotalAmount = model.TotalAmount,
                    // Avoid assigning TotalPrice manually; it will be calculated
                    CartItems = model.CartItems.Select(item => new OrderItem
                    {
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        UnitPrice = item.UnitPrice, // Set the unit price
                        ProductId = item.ProductId,
                        Product = item.Product, // Assuming you have a valid Product object
                                                // Do not manually set TotalPrice here; it will be calculated automatically
                    }).ToList()
                };


                _orderService.PlaceOrder(order);

                // Redirect to a success page or confirmation page
                return RedirectToAction("OrderConfirmation");
            }

            // If validation fails, return to the checkout page
            return View("Index", model);
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}
