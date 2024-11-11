using ECOMM.Business.Abstract;
using ECOMM.Business.Concrete;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Controllers
{
    public class FavouritesController : Controller
    {
        //  private readonly IFavouritesService favouritesService;
        private readonly IUserService userService;
    private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;
        private readonly ISessionService _sessionService;
        public FavouritesController(IUserService userService, IProductService productService, ISubCategoryService subCategoryService, ICategoryService categoryService, IFavouritesService favouritesService, ISessionService sessionService)
        {
            this.userService = userService;
            this.productService = productService;
            this.subCategoryService = subCategoryService;
            this.categoryService = categoryService;
            _sessionService = sessionService;
            //  this.favouritesService = favouritesService;
        }


        public IActionResult Index()
        {
            var cartItems = _sessionService.GetCartItems(); // Oturumdan sepet öğelerini al
            return View(cartItems); // Sepet sayfasına gönder
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await productService.GetByIdAsync(productId); // Ürünü al
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

            return RedirectToAction("Index", "Favourites"); // Sepet sayfasına yönlendir
        }
    }
}
