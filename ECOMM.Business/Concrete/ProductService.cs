﻿using ECOMM.Core.Models;
using ECOMM.Business.Abstract;
using ECOMM.Data.Shared.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECOMM.Business.Concrete
{
    public class ProductService : Service<Product>,IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ICategoryService _categoryService;
        public ProductService(IRepository<Product> productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }
        //public async Task<Product> GetByIdAsync(int id)
        //{
        //    return await _productRepository.GetByIdAsync(id).Include(p => p.Category)
        //                .FirstOrDefaultAsync(p => p.Id == id);
        //}
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.Query()
                                           .Include(p => p.Category)
                                           .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Product>> GetPaginatedProductsAsync(int page, int pageSize)
        {
            return await _productRepository.Query()
                                           .Include(p => p.Category)
                                           .Skip((page - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() // Kategorileri döndüren metot
        {
            return await _categoryService.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetBySubCategoryIdAsync(int subCategoryId)
        {
            
            var products = await _productRepository.GetAllAsync();
            return products.Where(p => p.Id == subCategoryId).ToList();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
          var products = await _productRepository.GetAllAsync();
            return products.Where(p=>p.Id == categoryId).ToList();
        }
    }
}
