using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
        }
    }
}
