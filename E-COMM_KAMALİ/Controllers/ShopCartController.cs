using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Core.ViewModels;
using System.Threading.Tasks;

namespace E_COMM_KAMALİ.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ISessionService _sessionService;

        public ShopCartController(IOrderService orderService, ICategoryService categoryService, IProductService productService, ISubCategoryService subCategoryService, ISessionService sessionService)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _productService = productService;
            _subCategoryService = subCategoryService;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            var cartItems = _sessionService.GetCartItems(); // Oturumdan sepet öğelerini al
            return View(cartItems); // Sepet sayfasına gönder
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _productService.GetByIdAsync(productId); // Ürünü al
            if (product != null)
            {
                var cartItem = new CartItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    ImagePath = product.ImagePath,
                    Price = product.ProductPrice,
                    Quantity = quantity
                };

                _sessionService.AddToCart(cartItem); // Sepete ekle
            }

            return RedirectToAction("Index", "ShopCart"); // Sepet sayfasına yönlendir
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _sessionService.RemoveFromCart(productId); // Sepetten ürünü kaldır
            return RedirectToAction("Index", "ShopCart"); // Sepet sayfasına yönlendir
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            _sessionService.ClearCart(); // Sepeti temizle
            return RedirectToAction("Index", "ShopCart"); // Sepet sayfasına yönlendir
        }
    }
}
