using ECOMM.Core.Models;
using ECOMM.Business.Abstract;
using ECOMM.Data.Shared.Abstract;
using Microsoft.EntityFrameworkCore;
using ECOMM.Core.ViewModels;

namespace ECOMM.Business.Concrete
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly ISubCategoryService subCategoryService;
        private readonly ICategoryService _categoryService;

        public ProductService(IRepository<Product> productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.Query()
                                           .Include(p => p.Category)
                                           .ThenInclude(c => c.SubCategories)
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

        #region Filtreleme
        public async Task<IEnumerable<Product>> GetBySubCategoryIdAsync(int subCategoryId)
        {

            var products = await _productRepository.GetAllAsync();
            return products.Where(p => p.Id == subCategoryId).ToList();
        }

        public async Task<List<ProductViewModel>> GetProductsByCategoryIdAsync(int categoryId)
        {
            // Kategorisine göre ürünleri alıyoruz
            var products = await _productRepository.GetAllAsync(); // Asenkron olarak tüm ürünleri alıyoruz

            var filteredProducts = products
                .Where(p => p.CategoryId == categoryId) // Kategorisine göre filtreliyoruz
                .Select(p => new ProductViewModel // ProductViewModel'e dönüştürüyoruz
                {
                    ProductId = p.Id,
                    ProductName = p.ProductName,
                    ProductSize = string.Join("", p.ProductSizes),
                    ProductDescription = p.ProductDescription,
                    Price = p.ProductPrice,
                    ImageUrl = p.ImagePath
                })
                .ToList(); // Listeye çeviriyoruz (asenkron olmadan)

            return filteredProducts; // Filtrelenmiş ürünleri döndürüyoruz
        }



        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepository.GetAllAsync();

            return products.Where(p => p.CategoryId == categoryId).ToList();

        }

        #endregion

        public async Task<List<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsyncQuery();
            return await products.Include(p => p.Category).Include(s => s.SubCategory)
                .Select(static p => new ProductViewModel
                {
                    ProductId = p.Id,
                    ImageUrl = p.ImagePath,
                    ImageUrl1 = p.ImagePath1,
                    ImageUrl2 = p.ImagePath2,
                    ImageUrl3 = p.ImagePath3,
                    ProductName = p.ProductName,
                    ProductTitle = p.ProductTitle,
                    ProductSize = string.Join(", ", p.ProductSizes),
                    DiscountRate = p.DiscountRate,
                    ProductDescription = p.ProductDescription,
                    Price = p.ProductPrice,
                    Category = new CategoryViewModel
                    {
                        ParentCategoryName = p.Category.ParentCategoryName
                    },
                    SubCategory = new SubCategoryViewModel
                    {
                        SubCategoryName = p.SubCategory.SubCategoryName
                    }
                }).ToListAsync();
        }

        //public async Task<Product> GetByIdAsync(int id)
        //{
        //    return await _productRepository.GetByIdAsync(id).Include(p => p.Category)
        //                .FirstOrDefaultAsync(p => p.Id == id);
        //}

    }
}
