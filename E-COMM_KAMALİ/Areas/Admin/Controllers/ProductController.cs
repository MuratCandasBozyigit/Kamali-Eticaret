﻿using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
     
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var Products = _productService.GetAllAsync();
            var categories = _categoryService.GetAllAsync();

          
            //var viewModel = new HomeViewModel
            //{
            //    Products = Products,
            //    Categories = categories
            //};

          
            return View(/*viewModel*/);
        }

        public class HomeViewModel
        {
            public IEnumerable<Product> Products { get; set; }
            public IEnumerable<Category> Categories { get; set; }
        }

        #region Tamamlandı 

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            _productService.GetAllAsync();
            return Ok();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            var categories = _categoryService.GetAllAsync(); 
            var viewModel = new HomeViewModel
            {
                Categories = (IEnumerable<Category>)categories
            };

            return View(viewModel);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromForm] Product Product, IFormFile image)
        {
            //var category = _categoryService.GetCategoriesById(Product.CategoryId);
            //Product.TagName = category.Tag.Name;

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                Product.ImagePath = "/images/" + fileName;
            }

            _productService.AddAsync(Product);
            return Json(new { success = true, message = "Product başarıyla kaydedildi." });



        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0) 
            {
                return BadRequest("Invalid Id Format");
            }

            var Product = _productService.GetByIdAsync(id);
            if (Product == null)
            {
                return NotFound("Product Not Found");
            }

            _productService.DeleteAsync(id);
            return Ok();
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


        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var Product = _productService.GetByIdAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            var categories = _categoryService.GetAllAsync();
            //var viewModel = new ProductEditViewModel
            //{
            //    Product = Product,
            //    Categories = categories
            //};

            return View(/*viewModel*/);
        }

        [HttpPost("Edit/{id}")]
        public IActionResult Edit(ProductEditViewModel viewModel, IFormFile image)
        {
            var ProductToUpdate = _productService.GetByIdAsync(viewModel.Product.Id);
            if (ProductToUpdate == null)
            {
                return NotFound();
            }

            //ProductToUpdate.Title = viewModel.Product.Title;
            //ProductToUpdate.Content = viewModel.Product.Content;
            //ProductToUpdate.CategoryId = viewModel.Product.CategoryId;
           
            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

               // ProductToUpdate.ImagePath = "/images/" + fileName;
            }

          //  _productService.UpdateAsync(ProductToUpdate);
            return Json(new { success = true, message = "Product başarıyla güncellendi." });
        }

        #endregion

    }
}







#region YORUMSATIRLARISSSS

//ASIL EDİT BU
//[HttpGet]
//public IActionResult Edit()
//{
//    return View();
//}
//[HttpProduct]
//public IActionResult Edit([FromBody] Product Product)
//{
//    if (Product == null)
//    {
//        return BadRequest("Product nesnesi null.");
//    }

//    if (!ModelState.IsValid)
//    {
//        return BadRequest("Product model doğrulama hatası.");
//    }

//    try
//    {
//        // Eğer 'GetAll' metodunun doğru bir kullanımı değilse, uygun metodu çağırmalısınız.
//        var Products = _productService.GetAll(); // Eğer 'Product' ile filtreleme yapılacaksa uygun metodu çağırmalısınız
//        return Ok(Products);
//    }
//    catch (Exception ex)
//    {
//        // Özel hata mesajları veya loglama
//        return StatusCode(500, $"Sunucu hatası: {ex.Message}");
//    }
//}


//[HttpGet]
//public IActionResult Edit()
//{
//    return View();
//}
//[HttpProduct]
//public IActionResult Edit([FromBody] Product Product)
//{
//    if (Product == null)
//    {
//        return BadRequest("Product nesnesi null.");
//    }

//    if (!ModelState.IsValid)
//    {
//        return BadRequest("Product model doğrulama hatası.");
//    }

//    try
//    {
//        // Eğer 'GetAll' metodunun doğru bir kullanımı değilse, uygun metodu çağırmalısınız.
//        var Products = _productService.GetAll(); // Eğer 'Product' ile filtreleme yapılacaksa uygun metodu çağırmalısınız
//        return Ok(Products);
//    }
//    catch (Exception ex)
//    {
//        // Özel hata mesajları veya loglama
//        return StatusCode(500, $"Sunucu hatası: {ex.Message}");
//    }
//}



//public IActionResult Create(Product Product, IFormFile image)
//{
//    if (ModelState.IsValid)
//    {
//        if (image != null && image.Length > 0)
//        {
//            var fileName = Path.GetFileName(image.FileName);
//            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                image.CopyTo(stream);
//            }

//            Product.ImagePath = "~/images/" + fileName;
//        }

//        _productService.Add(Product);
//        return RedirectToAction("Index");
//    }

//    return View(Product);
//}









//[HttpGet("{id}")]
//public IActionResult Edit(int id)
//{
//    var Product = _productService.GetById(id);
//    if (Product == null)
//    {
//        return NotFound();
//    }

//    var categories = _categoryService.GetAll();
//    var viewModel = new ProductEditViewModel
//    {
//        Product = Product,
//        Categories = categories
//    };

//    return View(viewModel);
//}

//[HttpProduct]
//public IActionResult Edit(ProductEditViewModel viewModel, IFormFile image)
//{
//    if (!ModelState.IsValid)
//    {
//        viewModel.Categories = _categoryService.GetAll(); // Kategorileri yeniden yükle
//        return View(viewModel);
//    }

//    if (image != null && image.Length > 0)
//    {
//        var fileName = Path.GetFileName(image.FileName);
//        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

//        using (var stream = new FileStream(filePath, FileMode.Create))
//        {
//            image.CopyTo(stream);
//        }

//        viewModel.Product.ImagePath = "/images/" + fileName;
//    }

//    _productService.Update(viewModel.Product); // Update metodunu çağırdığınızdan emin olun
//    return RedirectToAction("Index");
//}

#endregion