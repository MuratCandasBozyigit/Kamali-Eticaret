using E_COMM_KAMALİ.Models;
using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_COMM_KAMALİ.Controllers
{
    public class HomeController : Controller
    {
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
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 15; // Sayfa başına 15 ürün
            var products = await _productService.GetAllAsync();

            // Sayfalama işlemi
            var paginatedProducts = products.Skip((page - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = Math.Ceiling((double)products.Count() / pageSize);

            return View(paginatedProducts);
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
