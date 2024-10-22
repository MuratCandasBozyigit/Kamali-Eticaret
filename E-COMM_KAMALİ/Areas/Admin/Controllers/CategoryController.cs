using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Business.Concrete;
using ECOMM.Core.ViewModels;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService; 

        public CategoryController(ICategoryService categoryService, ISubCategoryService subCategoryService)
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService; 
        }

        #region Kategori İşlemleri

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return Json(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategoriler Listelenemedi: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Geçersiz Kategori Id'si.");
            }

            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategori Getirilemedi: {ex.Message}");
            }
        }

        [HttpPost("AddAsync")]
        public async Task<IActionResult> AddAsync([FromBody] Category category)
        {
            if (category == null || !ModelState.IsValid)
            {
                return BadRequest("Geçersiz Kategori Verisi.");
            }

            try
            {
                var createdCategory = await _categoryService.AddAsync(category);
                return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategori Eklenemedi: {ex.Message}");
            }
        }


        [HttpPut("UpdateAsync")]///{id}
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Category category)
        {
            //if (category == null || category.Id != id || !ModelState.IsValid)
            //{
            //    return BadRequest("Geçersiz Kategori Verisi.");
            //}

            try
            {
                await _categoryService.UpdateAsync(category);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategori Güncellenemedi: {ex.Message}");
            }
        }

        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Geçersiz Kategori Id'si.");
            }

            try
            {
                var result = await _categoryService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Kategori bulunamadı.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategori Silinemedi: {ex.Message}");
            }
        }

        #endregion

        #region Alt Kategori İşlemleri

        //// Alt Kategorilerin listelenmesi
        //[HttpGet("SubCategoryIndex")]
        //public async Task<IActionResult> SubCategoryIndex()
        //{
        //    var subCategories = await _subCategoryService.GetAllAsync();
        //    return View(subCategories);
        //}

        [HttpGet("SubCategoryIndex")]
        public async Task<IActionResult> SubCategoryIndex()
        {
            // Alt kategorileri al
            var subCategories = await _subCategoryService.GetAllIncludingCategoryAsync();

            // SubCategory modelinden SubCategoryViewModel'e dönüşüm yapıyoruz
            var subCategoryViewModels = subCategories.Select(sc => new SubCategoryViewModel
            {
                Id = sc.Id,
                SubCategoryName = sc.SubCategoryName,
                CategoryId = sc.CategoryId,
                CategoryName = sc.Category.ParentCategoryName // Ana kategorinin adını alıyoruz
            }).ToList();

            // Kategorileri de al ve View'a geç
            var categories = await _categoryService.GetAllAsync(); // Kategorileri getir

            ViewBag.Categories = categories; // Kategorileri ViewBag'e ekliyoruz

            return View(subCategoryViewModels);
        }



        // Alt kategori ekleme
        [HttpPost("AddSubCategory")]
        public async Task<IActionResult> AddSubCategory([FromBody] SubCategory subCategory)
        {
            if (subCategory == null || !ModelState.IsValid)
            {
                return BadRequest("Geçersiz Alt Kategori Verisi.");
            }

            // Alt kategorinin bağlı olduğu ana kategoriyi kontrol et
            var category = await _categoryService.GetByIdAsync(subCategory.CategoryId);
            if (category == null)
            {
                return BadRequest("Bağlı olduğu ana kategori bulunamadı.");
            }

            try
            {
                await _subCategoryService.AddAsync(subCategory);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Alt kategori eklenirken hata oluştu: {ex.Message}");
            }
        }

        // Alt kategori güncelleme
        [HttpPost("UpdateSubCategory")]
        public async Task<IActionResult> UpdateSubCategory([FromBody] SubCategory subCategory)
        {
            if (subCategory == null || !ModelState.IsValid)
            {
                return BadRequest("Geçersiz Alt Kategori Verisi.");
            }

            // Güncellenen alt kategorinin bağlı olduğu ana kategoriyi kontrol et
            var category = await _categoryService.GetByIdAsync(subCategory.CategoryId);
            if (category == null)
            {
                return BadRequest("Bağlı olduğu ana kategori bulunamadı.");
            }

            try
            {
                await _subCategoryService.UpdateAsync(subCategory);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Alt kategori güncellenirken hata oluştu: {ex.Message}");
            }
        }

        // Alt kategori silme
        [HttpPost("DeleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Geçersiz Alt Kategori Id'si.");
            }

            try
            {
                var result = await _subCategoryService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Alt kategori bulunamadı.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Alt kategori silinirken hata oluştu: {ex.Message}");
            }
        }

        #endregion
    }
}
