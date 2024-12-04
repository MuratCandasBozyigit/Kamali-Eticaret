using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class ProductController : Controller
    {
        #region Servisler 
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
       

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
       
        }

        #endregion

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 6; // Sayfada gösterilecek ürün sayısı
            var allProducts = await _productService.GetAllAsync(); // Tüm ürünleri yükle

            // Eğer allProducts IEnumerable olarak geliyorsa, Count'lamadan önce ToList() ile listeye çeviriyoruz
            var productList = allProducts.ToList(); // Sayfada işlem yapabilmek için listeye çevir
            var totalCount = productList.Count; // Toplam ürün sayısı

            // Mevcut sayfaya göre ürünleri al
            var products = productList
                .Skip((page - 1) * pageSize) // Geçerli sayfayı atla
                .Take(pageSize) // Belirli sayıda ürün al
                .ToList();

            var categories = await _categoryService.GetAllAsync(); // Kategorileri de yükle
            var model = new HomeViewModel
            {
                Products = products,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                Categories = categories.ToList() // Kategorileri listeye çevir
            };

            return View(model);
        }


        public class HomeViewModel
        {
            public List<Product> Products { get; set; }
            public List<Category> Categories { get; set; }
            public int TotalCount { get; set; }
            public int PageSize { get; set; } = 6;
            public int CurrentPage { get; set; } = 1;
            public List<string> ProductSizes { get; set; } = new List<string>();
            public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        }

        #region Tamamlandı 
        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                var categories = await _categoryService.GetAllAsync();

                foreach (var product in products)
                {
                    product.Category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
                }

                return Json(products); // JSON formatında döndür
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }


        [HttpGet("GetAllProductsAsync")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();
            return Json(products); // JSON olarak döndürme
        }

        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Geçersiz Ürün Id'si.");
            }

            try
            {
                var result = await _productService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Ürün bulunamadı.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Ürün Silinemedi: {ex.Message}");
            }
        }

        [HttpGet("GetById{id}")]
        public IActionResult GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Geçerli ıd yok ");
            }
            try
            {
                var Product = _productService.GetByIdAsync(id);
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "sea sorun getbyıd");
            }
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            var viewModel = new ProductEditViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ParentCategoryName
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Product product, IFormFile image, [FromForm] List<string> ProductSizes, [FromForm] Dictionary<string, int> SizeStock, double? DiscountRate)
        {
            if (!ProductSizes.Any() || SizeStock == null || !SizeStock.Any())
            {
                ModelState.AddModelError("ProductSizes", "En az bir beden ve stok bilgisi eklemelisiniz.");
                return View();
            }

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                product.ImagePath = "/images/" + fileName;
            }

            product.ProductSizes = ProductSizes;
            product.SizeStock = SizeStock;
            product.DiscountRate = DiscountRate;

            await _productService.AddAsync(product);
            return RedirectToAction("Index");
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = await _categoryService.GetAllAsync();

            var viewModel = new ProductEditViewModel
            {
                ProductId = product.Id,
                ProductTitle = product.ProductTitle,
                ProductName = product.ProductName,
                ProductSizes = product.ProductSizes,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ImagePath = product.ImagePath,
                CategoryId = product.CategoryId,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ParentCategoryName
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(ProductEditViewModel viewModel, IFormFile image, [FromForm] List<string> ProductSizes, [FromForm] Dictionary<string, int> SizeStock)
        {
            var productToUpdate = await _productService.GetByIdAsync(viewModel.ProductId);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            productToUpdate.ProductTitle = viewModel.ProductTitle;
            productToUpdate.ProductName = viewModel.ProductName;
            productToUpdate.ProductDescription = viewModel.ProductDescription;
            productToUpdate.ProductPrice = viewModel.ProductPrice;
            productToUpdate.CategoryId = viewModel.CategoryId;
            productToUpdate.ProductSizes = ProductSizes;
            productToUpdate.SizeStock = SizeStock;
            productToUpdate.DateUpdated = DateTime.UtcNow;

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                productToUpdate.ImagePath = "/images/" + fileName;
            }

            await _productService.UpdateAsync(productToUpdate);
            return RedirectToAction("Index");
        }

        #endregion
    }


}






