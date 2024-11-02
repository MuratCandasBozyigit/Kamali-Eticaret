using E_COMM_KAMALİ.Models;
using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http; // Eklenmeli

public static class HttpRequestExtensions
{
    public static bool IsAjaxRequest(this HttpRequest request)
    {
        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
}

namespace E_COMM_KAMALİ.Controllers
{
    public class HomeController : Controller
    {
        #region Servisler 
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        // private readonly IFavouritesService _favouritesService;
        private readonly ICommentService _commentService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IOrderService orderService, /*IFavouritesService favouritesService,*/ ICommentService commentService)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            //  _favouritesService = favouritesService;
            _commentService = commentService;
        }
        #endregion

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 4;
            try
            {
                var products = await _productService.GetAllAsync();
                var paginatedProducts = products.Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = Math.Ceiling((double)products.Count() / pageSize);


                // Eğer AJAX isteği ise, yalnızca ürün listesini döndür
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_ProductListPartial", paginatedProducts); // Yalnızca ürün listesini döndür
                }

                // Tam sayfa görünüm döndür
                return View(paginatedProducts);
            }
            catch (Exception ex)
            {
                // Hata kaydı yap
                _logger.LogError(ex, "Ürünler yüklenirken hata oluştu.");
                return StatusCode(500, "Ürünler yüklenirken bir hata oluştu.");
            }
        }



        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _productService.GetAllAsync();
            ViewBag.Categories = categories;
            return View(categories);
        }

        #region s

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
