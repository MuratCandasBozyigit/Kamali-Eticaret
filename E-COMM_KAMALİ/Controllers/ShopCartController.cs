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
        public async Task<IActionResult> AddToCart(int productId, string selectedSize, int quantity)
        {
            var product = await _productService.GetByIdAsync(productId); // Ürünü al
            if (product != null)
            {
                var cart = _sessionService.GetCartItems(); // Kullanıcının mevcut sepetini al
                var existingItem = cart.FirstOrDefault(c => c.ProductId == productId && c.ProductSize == selectedSize);

                if (existingItem != null)
                {
                    // Eğer ürün ve beden zaten sepetteyse, toplam miktarı kontrol et
                    int newQuantity = existingItem.Quantity + quantity;
                    if (newQuantity > 5)
                    {
                        existingItem.Quantity = 5; // Maksimum 5 ile sınırla
                    }
                    else
                    {
                        existingItem.Quantity = newQuantity;
                    }
                }
                else
                {
                    // Yeni bir ürün ve beden ekleniyorsa
                    if (quantity > 5) quantity = 5;
                    var cartItem = new CartItemViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.ProductName,
                        ProductSize = selectedSize,  // Seçilen beden
                        ImagePath = product.ImagePath,
                        Price = product.ProductPrice,
                        Quantity = quantity
                    };

                    _sessionService.AddToCart(cartItem);
                }
            }

            return RedirectToAction("Index", "ShopCart");
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

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            if (quantity >= 1 && quantity <= 5)
            {
                await _sessionService.UpdateQuantityAsync(productId, quantity); // Miktarı güncelle
                return Json(new { success = true }); // Başarılı dönüş
            }

            return Json(new { success = false, message = "Miktar 1 ile 5 arasında olmalıdır." }); // Hata durumu
        }


    }
}
