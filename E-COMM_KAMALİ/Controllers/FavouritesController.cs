using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Controllers
{
    public class FavouritesController : Controller
    {
        private readonly IFavouritesService favouritesService;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly ISubCategoryService subCategoryService;

        public IActionResult Index()
        {
            return View();
        }
    }
}
