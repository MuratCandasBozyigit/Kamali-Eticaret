
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

      
        [HttpGet("GetAllCategories")]
        public IActionResult GetAll()
        {
            try
            {
                var categories = _categoryService.GetAllAsync();
                return Json(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return BadRequest("Invalid ID format.");
            }

            var iD = _categoryService.GetFirstOrDefaultAsync(i => i.Id == id);
            if (iD == null)
            {
                return NotFound("Color not found.");
            }

            _categoryService.DeleteAsync(iD.Id);
            return Ok(iD);
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            if (category == null || category.Id != id)
            {
                return BadRequest("Invalid category data.");
            }

            try
            {
                _categoryService.UpdateAsync(category);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid Id Format");
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPost("Add")]
        public IActionResult Add([FromBody]Category category)
        {
            
                try
                {
             
                   var categories = _categoryService.AddAsync(category);
                    return Ok(categories);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal category server error: {ex.Message}");
                }

        }




        #endregion

    }
}
