using ECOMM.Business.Abstract;
using ECOMM.Business.Concrete;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // Ürün Listesi
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 6; // Sayfada gösterilecek ürün sayısı
            var allProducts = await _productService.GetAllAsync(); // Tüm ürünleri yükle
            var productList = allProducts.ToList(); // Listeye çevir
            var totalCount = productList.Count; // Toplam ürün sayısı

            // Mevcut sayfaya göre ürünleri al
            var products = productList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var categories = await _categoryService.GetAllAsync(); // Kategorileri de yükle
            var model = new HomeViewModel
            {
                Products = products,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                Categories = categories.ToList()
            };

            return View(model);
        }

        // Ürünleri Getir (AJAX)
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync(); // Tüm ürünleri al
            var productList = products.ToList(); // Listeye çevir
            var categories = await _categoryService.GetAllAsync(); // Tüm kategorileri al

            var result = new
            {
                data = productList.Select(p => new
                {
                    p.Id,
                    p.ProductTitle,
                    p.ProductDescription,
                    p.ProductPrice,
                    p.ImagePath,
                    category = new
                    {
                        parentCategoryName = categories.FirstOrDefault(c => c.Id == p.CategoryId)?.ParentCategoryName
                    }
                }),
                recordsTotal = productList.Count,
                recordsFiltered = productList.Count
            };

            return Json(result);
        }

        // Ürün Ekle
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            var viewModel = new HomeViewModel
            {
                Categories = categories.ToList()
            };

            return View(viewModel);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Product product, IFormFile image)
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

            await _productService.AddAsync(product);
            return RedirectToAction("Index");
        }

        // Ürün Sil
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

        // Ürün Düzenle
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
                ProductDescription = product.ProductDescription,
                ProductPrice = (decimal)product.ProductPrice,
                ImagePath = product.ImagePath,
                CategoryId = product.Category?.Id ?? 0,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.ParentCategoryName
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(ProductEditViewModel viewModel, IFormFile image)
        {
            var productToUpdate = await _productService.GetByIdAsync(viewModel.ProductId);
            if (productToUpdate == null)
            {
                return NotFound();
            }

            productToUpdate.ProductTitle = viewModel.ProductTitle;
            productToUpdate.ProductDescription = viewModel.ProductDescription;
            productToUpdate.ProductPrice = viewModel.ProductPrice;
            productToUpdate.CategoryId = viewModel.CategoryId;

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
    }
}