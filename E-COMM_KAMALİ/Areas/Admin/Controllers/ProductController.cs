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
       private readonly ISubCategoryService _subCategoryService;

        public ProductController(IProductService productService, ICategoryService categoryService, ISubCategoryService subCategoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
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

            return View("Index");
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
                var subCategories = await _subCategoryService.GetAllAsync();

                foreach (var product in products)
                {
                    // Set the category of the product
                    product.Category = categories.FirstOrDefault(c => c.Id == product.CategoryId);

                    // Set the subcategory of the product
                    product.SubCategory = subCategories.FirstOrDefault(s => s.Id == product.SubCategory.Id);
                }

                return Json(products); // Return the products in JSON format
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
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
            var subCategories = await _subCategoryService.GetAllAsync();

            // Check for null or empty categories or subCategories
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "No categories found.");
                return View(new ProductCreateViewModel());
            }

            if (subCategories == null || !subCategories.Any())
            {
                ModelState.AddModelError("", "No subcategories found.");
            }

            var viewModel = new ProductCreateViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ParentCategoryName
                }).ToList(),
                SubCategories = subCategories.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.SubCategoryName
                }).ToList() // Initialize as an empty list if null
            };

            return View(viewModel);
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(Product product, IFormFile image, IFormFile image1, IFormFile image2, IFormFile image3, [FromForm] List<string> ProductSizes, [FromForm] Dictionary<string, int> SizeStock, double? DiscountRate)
        {
            ProductSizes = ProductSizes ?? new List<string>();
            SizeStock = SizeStock ?? new Dictionary<string, int>();

            if (!ProductSizes.Any() || !SizeStock.Any())
            {
                ModelState.AddModelError("ProductSizes", "En az bir beden ve stok bilgisi eklemelisiniz.");
                return View(product);
            }

            try
            {
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
                if (image1 != null && image1.Length > 0)
                {
                    var fileName = Path.GetFileName(image1.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image1.CopyToAsync(stream);
                    }

                    product.ImagePath1 = "/images/" + fileName;
                  
                }
                if (image2 != null && image2.Length > 0)
                {
                    var fileName = Path.GetFileName(image2.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image2.CopyToAsync(stream);
                    }

                    product.ImagePath2 = "/images/" + fileName;
                  
                }
                if (image3 != null && image3.Length > 0)
                {
                    var fileName = Path.GetFileName(image3.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image3.CopyToAsync(stream);
                    }

                    product.ImagePath3 = "/images/" + fileName;
                }


                product.ProductSizes = ProductSizes;
                product.SizeStock = SizeStock;
                product.DiscountRate = DiscountRate;

                await _productService.AddAsync(product);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // Log the exception message
                ModelState.AddModelError("", "Bir hata oluştu. Lütfen tekrar deneyin.");
                return View(product);
            }
        }


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            // Fetch the product by its id
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Fetch all subcategories based on the selected category of the product
            var subCategories = await _subCategoryService.GetByCategoryIdAsync(product.CategoryId);
            if (subCategories == null || !subCategories.Any())
            {
                ModelState.AddModelError("", "No subcategories found for this category.");
                return View("Error");
            }

            // Fetch all categories
            var categories = await _categoryService.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "No categories found.");
                return View("Error");
            }

            // Prepare the view model with product details
            var viewModel = new ProductEditViewModel
            {
                ProductId = product.Id,
                ProductTitle = product.ProductTitle,
                ProductName = product.ProductName,
                ProductSizes = product.ProductSizes ?? new List<string>(), // Null check for sizes
                DiscountRate = product.DiscountRate,
                ProductDescription = product.ProductDescription,
                ProductPrice = product.ProductPrice,
                ImagePath = product.ImagePath,
                ImagePath1 = product.ImagePath1,
                ImagePath2 = product.ImagePath2,
                ImagePath3 = product.ImagePath3,
                CategoryId = product.CategoryId, // Set the current category
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ParentCategoryName // Assuming 'ParentCategoryName' is the property to display
                }).ToList(),
                SubCategoryId = product.SubCategoryId, // Set the current subcategory
                SubCategories = subCategories.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.SubCategoryName // Assuming 'SubCategoryName' is the property to display
                }).ToList(),
            };

            return View(viewModel);
        }


        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(ProductEditViewModel viewModel, IFormFile image, IFormFile image1, IFormFile image2, IFormFile image3, [FromForm] List<string> ProductSizes, [FromForm] Dictionary<string, int> SizeStock)
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
            productToUpdate.SubCategoryId = viewModel.SubCategoryId;
            productToUpdate.ProductSizes = ProductSizes ?? productToUpdate.ProductSizes;
            productToUpdate.DiscountRate = viewModel.DiscountRate;
            productToUpdate.SizeStock = SizeStock ?? productToUpdate.SizeStock;
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
            else
            {
                productToUpdate.ImagePath = productToUpdate.ImagePath; // Var olan yolu koru
            }


            if (image1 != null && image1.Length > 0)
            {
                var fileName = Path.GetFileName(image1.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image1.CopyToAsync(stream);
                }
                productToUpdate.ImagePath1 = "/images/" + fileName;
            
            }
            else
            {
                productToUpdate.ImagePath = productToUpdate.ImagePath; // Var olan yolu koru
            }

            if (image2 != null && image2.Length > 0)
            {
                var fileName = Path.GetFileName(image2.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image2.CopyToAsync(stream);
                }

                productToUpdate.ImagePath2 = "/images/" + fileName;
            }
            else
            {
                productToUpdate.ImagePath = productToUpdate.ImagePath; // Var olan yolu koru
            }

            if (image3 != null && image3.Length > 0)
            {
                var fileName = Path.GetFileName(image3.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image3.CopyToAsync(stream);
                }
                productToUpdate.ImagePath3 = "/images/" + fileName;
            }
            else
            {
                productToUpdate.ImagePath = productToUpdate.ImagePath; // Var olan yolu koru
            }

            await _productService.UpdateAsync(productToUpdate);
            return RedirectToAction("Index");
        }

        #endregion
    }


}






