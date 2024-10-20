
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
   
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
     
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
         
        }

        #region Tamamlandı 

        [HttpGet]
        public IActionResult Index()
        {
        
            return View();
        }

      
        [HttpGet("GetAllAsync")]
        public IActionResult GetAllAsync()
        {
            try
            {
                var categories = _categoryService.GetAllAsync();
                return Json(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategoriler Listelenemedi: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return BadRequest("Geçersiz Kategori Id'si.");
            }

            var iD = _categoryService.GetFirstOrDefaultAsync(i => i.Id == id);
            if (iD == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            _categoryService.DeleteAsync(iD.Id);
            return Ok(iD);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            if (category == null || category.Id != id)
            {
                return BadRequest("Geçersiz Kategori Verisi.");
            }

            try
            {
                _categoryService.UpdateAsync(category);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategoriler Listelenemedi:  {ex.Message}");
            }
        }


        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Geçersiz Kategori Veri Tipi.");
            }
            try
            {
                var category = _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Hatası Kategoriler Listelenemedi:   {ex.Message}");
            }
        }



        [HttpPost("AddAsync")]
        public IActionResult AddAsync([FromBody]Category category)
        {
            
                try
                {
             
                   var categories = _categoryService.AddAsync(category);
                    return Ok(categories);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Server Hatası Kategoriler Listelenemedi: {ex.Message}");
                }

        }




        #endregion

    }
}
