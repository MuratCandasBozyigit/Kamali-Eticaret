using E_COMM_KAMALİ.Models;
using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http; // Eklenmeli
using Microsoft.EntityFrameworkCore;
using ECOMM.Core.ViewModels;
using ECOMM.Core.Models;
using ECOMM.Business.Concrete;
using Microsoft.AspNetCore.Identity; 
//using Microsoft.AspNet.Identity;
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
        private readonly UserManager<User> _userManager;

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        // private readonly IFavouritesService _favouritesService;
        private readonly ICategoryService categoryService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IOrderService orderService, /*IFavouritesService favouritesService,*/ ICommentService commentService, IUserService userService, ICategoryService categoryService)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            //  _favouritesService = favouritesService;
            _commentService = commentService;
            _userService = userService;
            this.categoryService = categoryService;
        
        }
        #endregion
        #region Comment
        [HttpPost]
        public async Task<IActionResult> AddComment(string productId, string content)
        {
            // Kullanıcının kimliği doğrulandıysa User.Identity.Name ile al
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcıyı UserService ile bul
            var user = await _userService.GetUserByUserNameAsync(userName);

            if (user == null)
            {
                // Eğer kullanıcı bulunamazsa, hata mesajı döndür
                return BadRequest("Kullanıcı bulunamadı.");
            }

            // Kullanıcı adı ya da tam adı al
            var fullName = user.FullName ?? user.UserName; // FullName yoksa UserName kullan

            // Yeni yorum oluştur
            var comment = new Comment
            {
                Content = content,
                AuthorId = user.Id,
                AuthorName = fullName, // Tam adı kullanıyoruz
                ProductId = int.Parse(productId),
                DateCommented = DateTime.Now
            };

            // Yorum ekle
            await _commentService.AddAsync(comment);

            return Ok("Yorum başarıyla eklendi.");
        }



        public async Task<IActionResult> PendingComments()
        {
            var pendingComments = await _commentService.GetPendingCommentsAsync();
            return View(pendingComments);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveComment(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();

            comment.IsApproved = true;
            await _commentService.UpdateAsync(comment);

            TempData["Message"] = "Yorum onaylandı.";
            return RedirectToAction("PendingComments");
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

        public async Task<IActionResult> Products(int productId, int categoryId)
        {
            try
            {

                var products = await _productService.GetAllAsync();
                if (products == null || !products.Any())
                {
                    return View(new List<ProductViewModel>()); // Boş bir liste döndür
                }

                // Product nesnelerini ProductViewModel'e dönüştür
                var productViewModels = products.Select(product => new ProductViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Price = product.ProductPrice,
                    ImageUrl = product.ImagePath,
                    CategoryName = product.Category != null ? product.Category.ParentCategoryName : "Kategori Yok",
                    ProductDescription = product.ProductDescription // Eğer Product modelinde bu özellik varsa
                }).ToList();

                return View(productViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürünler yüklenirken hata oluştu.");
                return StatusCode(500, "Ürünler yüklenirken bir hata oluştu.");
            }
        }

        public async Task<IActionResult> ProductsByCategory(int categoryId)
        {
            var category = await categoryService.GetCategoryWithProductsAsync(categoryId); // Kategoriyi ve ilgili ürünleri alıyoruz
            if (category == null)
            {
                return NotFound();
            }

            return View(category); // CategoryViewModel ile gelen kategori ve ürünleri view'a gönderiyoruz
        }

        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _productService.GetAllAsync();
            ViewBag.Categories = categories;
            return View(categories);
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
                ProductTitle = product.ProductTitle,
                Price = product.ProductPrice,
                ImageUrl = product.ImagePath,
                CategoryName = product.Category != null ? product.Category.ParentCategoryName : "Kategori Yok",
                ProductDescription = product.ProductDescription
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
