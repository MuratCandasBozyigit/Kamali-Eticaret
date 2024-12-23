﻿using ECOMM.Business.Abstract;
using ECOMM.Core.ViewModels;
using ECOMM.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_COMM_KAMALİ.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly ISubCategoryService _subCategoryService;

        public CategoryController(ICategoryService categoryService, IProductService productService, ISubCategoryService subCategoryService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _subCategoryService = subCategoryService;
        }

        // Ana kategorileri listeleyen index metodu
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> SubCategories(int categoryId)
        {
            var category = await _categoryService.GetByIdAsyncWithSubCategories(categoryId); // Eager loading ile alt kategorileri de yükle
            if (category == null)
            {
                return NotFound();
            }

            // Tüm ürünleri al
            var allProducts = await _productService.GetAllAsync();

            // Her alt kategori için ürün sayısını hesapla
            foreach (var subCategory in category.SubCategories)
            {
                // Alt kategoriye ait ürünlerin sayısını hesapla
                subCategory.ProductCount = allProducts.Count(p => p.Id == subCategory.Id);
            }

            return View(category.SubCategories); // Alt kategorileri doğrudan döndür
        }

        public async Task<IActionResult> Products(int subCategoryId)
        {
            // Seçilen alt kategori bilgilerini al
            var subCategory = await _subCategoryService.GetByIdAsync(subCategoryId);
            if (subCategory == null)
            {
                return NotFound();
            }

            // Alt kategoriye ait tüm ürünleri al
            var products = await _productService.GetBySubCategoryIdAsync(subCategoryId);

            // View'a model olarak alt kategori ve ürün listesini gönder
            var model = new SubCategoryProductsViewModel
            {
                SubCategory = subCategory,
                Products = products
            };

            return View(model);
        }


    }
}
