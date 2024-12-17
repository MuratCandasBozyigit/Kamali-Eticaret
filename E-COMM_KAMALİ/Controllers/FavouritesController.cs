using ECOMM.Core.ViewModels;
using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Controllers
{
    public class FavouritesController : Controller
    {
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;
        private readonly ISessionService _sessionService;

        // FavouritesService artık kullanılmıyor. Bu, favori işlemlerini session ile yönetiyoruz.
        public FavouritesController(
            IUserService userService,
            IProductService productService,
            ISubCategoryService subCategoryService,
            ICategoryService categoryService,
            ISessionService sessionService)
        {
            this.userService = userService;
            this.productService = productService;
            this.subCategoryService = subCategoryService;
            this.categoryService = categoryService;
            _sessionService = sessionService;
        }

        // Favori ürünler listesi
        public IActionResult Index()
        {
            var favouritesItems = _sessionService.GetFavouritesItems(); // Oturumdan favori öğelerini al
            return View(favouritesItems); // Favori sayfasına gönder
        }



        // Favorilere ürün ekle
        [HttpPost]
        public async Task<IActionResult> AddToFavourites(int productId, string selectedSize)
        {
           

            var product = await productService.GetByIdAsync(productId);
            if (product != null)
            {
                var cartItem = new CartItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    ProductSize = selectedSize,
                    ImagePath = product.ImagePath,
                    Price = product.ProductPrice,
                };

                _sessionService.AddToFavourites(cartItem);
            }

            return RedirectToAction("Index", "Favourites");
        }



        // Favoriden ürün sil
        [HttpPost]
        public IActionResult RemoveFromFavourites(int productId)
        {
            _sessionService.RemoveFromFavourites(productId); // Favorilerden ürünü kaldır
            return RedirectToAction("Index", "Favourites"); // Favori sayfasına yönlendir
        }

        // Favorileri temizle
        [HttpPost]
        public IActionResult ClearFavourites()
        {
            _sessionService.ClearFavourites(); // Tüm favorileri temizle
            return RedirectToAction("Index", "Favourites"); // Favori sayfasına yönlendir
        }
    }
}
