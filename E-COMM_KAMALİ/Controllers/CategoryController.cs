using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_COMM_KAMALİ.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;

        public CategoryController(ICategoryService categoryService, IProductService productService, ISubCategoryService subCategoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _subCategoryService = subCategoryService;
        }

        // Ana kategorileri listeleyen index metodu
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> SubCategories(int categoryId)
        {
            var category = await _categoryService.GetByIdAsyncWithSubCategories(categoryId); // Eager loading ile alt kategorileri de yükle
            if (category == null)
            {
                return NotFound();
            }

            return View(category.SubCategories); // Alt kategorileri doğrudan döndür
        }
    }
}
