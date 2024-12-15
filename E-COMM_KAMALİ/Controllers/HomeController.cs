using E_COMM_KAMALİ.Models;
using ECOMM.Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ECOMM.Core.ViewModels;
using ECOMM.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace E_COMM_KAMALİ.Controllers
{
    public class HomeController : Controller
    {
        #region Servisler 
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IRazorViewEngine _viewEngine;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        // private readonly IFavouritesService _favouritesService;
        private readonly ICategoryService categoryService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly IModelMetadataProvider _metadataProvider;
        public HomeController(ILogger<HomeController> logger, IProductService productService, IOrderService orderService, /*IFavouritesService favouritesService,*/ ICommentService commentService, IUserService userService, ICategoryService categoryService, IRazorViewEngine viewEngine, IHttpContextAccessor httpContextAccessor, ITempDataProvider tempDataProvider, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, IModelMetadataProvider metadataProvider)
        {
            _logger = logger;
            _productService = productService;
            _orderService = orderService;
            //  _favouritesService = favouritesService;
            _commentService = commentService;
            _userService = userService;
            this.categoryService = categoryService;
            _viewEngine = viewEngine;
            _httpContextAccessor = httpContextAccessor;
            _tempDataProvider = tempDataProvider;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _metadataProvider = metadataProvider;
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

            return Ok("Yorum onay bekliyor.");
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
        #region Sayfalar
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                // Yorumları al ve yalnızca onaylı olanları filtrele
                var comments = await _commentService.GetAllAsync();
                var approvedComments = comments.Where(c => c.IsApproved).ToList();

                // Yorumları ViewModel'e dönüştür
                var commentsWithAuthors = approvedComments.Select(c => new CommentViewModel
                {
                    Content = c.Content,
                    AuthorName = c.AuthorName ?? "Bilinmiyor",
                    Author = c.Author?.FullName ?? "Bilinmiyor",  // Eğer Author null ise "Bilinmiyor" yaz
                    DateCommented = c.DateCommented
                }).ToList();

                int pageSize = 4;  // Sayfa başına gösterilecek ürün sayısı

                // Ürünleri al ve sayfalama işlemi yap
                var products = await _productService.GetAllAsync();
                var paginatedProducts = products.Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToList();

                // ViewModel oluştur
                var viewModel = new IndexViewModel
                {
                    Comments = commentsWithAuthors,  // Yorumları ViewModel'e atıyoruz
                    Products = paginatedProducts     // Ürünleri ViewModel'e atıyoruz
                };

                // Sayfa bilgilerini View'a gönder
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = Math.Ceiling((double)products.Count() / pageSize);

                // Sayfa görünümünü döndür
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

        public async Task<IActionResult> ProductDetails(int productId, int categoryId, int page = 1)
        {
            int pageSize = 4;

            // Tüm ürünleri al ve toplam ürün sayısını hesapla
            var products = await _productService.GetAllAsync();
            int totalProducts = products.Count(); // Toplam ürün sayısını al
            int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize); // Toplam sayfa sayısını hesapla

            // Sayfalı ürünleri al
            var paginatedProducts = await _productService.GetPaginatedProductsAsync(page, pageSize);

            // `Product` modelinden `ProductViewModel` modeline dönüşüm yap
            var paginatedProductViewModels = paginatedProducts.Select(p => new ProductViewModel
            {
                ProductId = p.Id,
                ProductName = p.ProductName,
                ProductSize = string.Join("", p.ProductSizes),
                ProductTitle = p.ProductTitle,
                Price = p.ProductPrice,
                ImageUrl = p.ImagePath,
                CategoryName = p.Category != null ? p.Category.ParentCategoryName : "Kategori Yok",
                ProductDescription = p.ProductDescription,
                DiscountRate = p.DiscountRate
            }).ToList();



            // Seçilen ürünü al
            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("İlgili ürün bulunamadı");
            }

            // İlgili ürünleri al
            var relatedProducts = await _productService.GetProductsByCategoryIdAsync(categoryId);
            // İlgili ürünleri ProductViewModel'e dönüştür
            var relatedProductViewModels = relatedProducts.Select(p => new ProductViewModel
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductTitle = p.ProductTitle,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                DiscountRate = p.DiscountRate
            }).ToList();
            // Yorumları al
            var comments = await _commentService.GetAllAsync();
            var commentsWithAuthors = comments.Select(c => new CommentViewModel
            {
                Content = c.Content,
                Author = c.Author?.UserName ?? "Bilinmiyor",
                DateCommented = c.DateCommented
            }).ToList();

            // Ürün modelini oluştur
            var productViewModel = new ProductViewModel
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                ProductSize = string.Join("", product.ProductSizes),
                ProductTitle = product.ProductTitle,
                Price = product.ProductPrice,
                ImageUrl = product.ImagePath,
                CategoryName = product.Category != null ? product.Category.ParentCategoryName : "Kategori Yok",
                ProductDescription = product.ProductDescription,

                DiscountRate = product.DiscountRate,  // DiscountRate'ı atıyoruz
                SubCategoryName = product.SubCategory != null ? product.SubCategory.SubCategoryName : "Alt Kategori Yok" // Handle subcategory similarly
            };


            var viewModel = new ProductDetailPageViewModel
            {
                Product = productViewModel,
                RelatedProducts = relatedProducts,
                Comments = commentsWithAuthors
            };

            // Normal sayfa döndürme (JSON yerine)
            return View(viewModel);

        }

        public async Task<IActionResult> GetProductCategories(int productId)
        {
            // Ürünü veritabanından alıyoruz
            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı" });
            }

            // Kategori ve Alt Kategori bilgilerini alıyoruz
            var categoryName = product.Category != null ? product.Category.ParentCategoryName : "Kategori Yok";
            var subCategoryName = product.SubCategory != null ? product.SubCategory.SubCategoryName : "Alt Kategori Yok";

            // JSON olarak döndürüyoruz
            return Json(new
            {
                success = true,
                categoryName = categoryName,
                subCategoryName = subCategoryName
            });
        }


        #endregion

        #region s

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
