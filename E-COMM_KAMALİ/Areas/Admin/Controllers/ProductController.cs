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

        //[HttpGet("Index")]
        //public IActionResult Index()
        //{
        //    var Products = _productService.GetAllAsync();
        //    var categories = _categoryService.GetAllAsync();


        //   var viewModel = new HomeViewModel
        //   {
        //       Products = Products,
        //       Categories = categories
        //   };


        //    return View(viewModel);
        //}
        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                Products =await _productService.GetAllAsync()
                
            }; 

            return View(model);
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
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync(); // Asenkron olarak bekle
            var viewModel = new HomeViewModel
            {
                Categories = categories // Artık IEnumerable<Category> türünde
            };

            return View(viewModel);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] Product Product, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream); // Asenkron kopyalama
                }

                Product.ImagePath = "/images/" + fileName;
            }

            await _productService.AddAsync(Product); // Asenkron olarak ekle
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
                Product = product,
                Categories = categories ?? new List<Category>() // Kategori listesini doğrudan atıyoruz
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

            // Güncellemeleri yap
            viewModel.UpdateProductInfo(productToUpdate); // Bilgileri güncelle

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream); // Asenkron kopyalama
                }

                productToUpdate.ImagePath = "/images/" + fileName; // Resim yolunu güncelle
            }

            await _productService.UpdateAsync(productToUpdate); // Ürünü güncelle
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