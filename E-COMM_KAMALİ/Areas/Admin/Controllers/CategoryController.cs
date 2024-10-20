using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Business.Concrete;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService; // Düzenlendi

        public CategoryController(ICategoryService categoryService, ISubCategoryService subCategoryService) // Constructor güncellendi
        {
            _categoryService = categoryService;
            _subCategoryService = subCategoryService; // Constructor'a eklendi
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

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (category == null || category.Id != id || !ModelState.IsValid)
            {
                return BadRequest("Geçersiz Kategori Verisi.");
            }

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

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
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

        [HttpGet("SubCategoryIndex")]
        public async Task<IActionResult> SubCategoryIndex()
        {
            var subCategories = await _subCategoryService.GetAllAsync();
            return View(subCategories);
        }

        [HttpPost("AddSubCategory")]
        public async Task<IActionResult> AddSubCategory(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subCategoryService.AddAsync(subCategory);
            return Ok();
        }

        [HttpPost("UpdateSubCategory")]
        public async Task<IActionResult> UpdateSubCategory(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subCategoryService.UpdateAsync(subCategory);
            return Ok();
        }

        [HttpPost("DeleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var result = await _subCategoryService.DeleteAsync(id);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }

        #endregion
    }
}
