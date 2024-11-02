using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace E_COMM_KAMALİ.Controllers
{
    public class ShopCartController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;


        public IActionResult Index()
        {
            return View();
        }
    }
}
