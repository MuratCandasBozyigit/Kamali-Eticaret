using E_COMM_KAMALİ.Models;
using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http; // Eklenmeli
using Microsoft.EntityFrameworkCore;
using ECOMM.Core.ViewModels;
//public static class HttpRequestExtensions
//{
//    public static bool IsAjaxRequest(this HttpRequest request)
//    {
//        return request.Headers["X-Requested-With"] == "XMLHttpRequest";
//    }
//}

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
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IOrderService orderService, /*IFavouritesService favouritesService,*/ ICommentService commentService, IUserService userService)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            //  _favouritesService = favouritesService;
            _commentService = commentService;
            _userService = userService;
        }
        #endregion

        public async Task<IActionResult> Index(int page = 1)
        {
            var comments = await _commentService.GetAllAsync();
            var commentsWithAuthors = comments.Select(c => new CommentViewModel
            {
                Content = c.Content,
                Author = c.Author?.UserName ?? "Bilinmiyor",
                DateCommented = c.DateCommented
            }).ToList();

            int pageSize = 4;
            try
            {
                var products = await _productService.GetAllAsync();
                var paginatedProducts = products.Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToList();

                var viewModel = new IndexViewModel
                {
                    Comments = commentsWithAuthors,
                    Products = paginatedProducts
                };

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = Math.Ceiling((double)products.Count() / pageSize);



                // Tam sayfa görünüm döndür
                return View(viewModel);
            }
            catch (Exception ex)
            {
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

        public async Task<IActionResult> Products(int productId)
        {
            return View();
        }

        public async Task<IActionResult> ProductDetails(int productId, int categoryId)
        {
            var category = await _productService.GetByCategoryIdAsync(categoryId);
            var categoryRelatedProducts = category.Select(product => new ProductViewModel
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.ProductPrice,
                ImageUrl = product.ImagePath,
                CategoryName = product.Category != null ? product.Category.ParentCategoryName : "Kategori Yok"
            }).ToList();

            var comments = await _commentService.GetAllAsync();
            var commentsWithAuthors = comments.Select(c => new CommentViewModel
            {
                Content = c.Content,
                Author = c.Author?.UserName ?? "Bilinmiyor",
                DateCommented = c.DateCommented
            }).ToList();

            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("İlgili ürün bulunamadı");
            }

            var productViewModel = new ProductViewModel
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Price = product.ProductPrice,
                ImageUrl = product.ImagePath,
                CategoryName = product.Category != null ? product.Category.ParentCategoryName : "Kategori Yok"
            };

            var viewModel = new ProductDetailPageViewModel
            {
                Product = productViewModel,
                RelatedProducts = categoryRelatedProducts,
                Comments = commentsWithAuthors
            };

            return View(viewModel);
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
