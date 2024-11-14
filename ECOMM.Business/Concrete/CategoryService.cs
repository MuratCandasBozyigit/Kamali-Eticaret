using System.Linq;
using System.Threading.Tasks;
using ECOMM.Core.Models;
using ECOMM.Business.Abstract;
using ECOMM.Data.Shared.Abstract;
using Microsoft.EntityFrameworkCore;
using ECOMM.Core.ViewModels;

namespace ECOMM.Business.Concrete
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IProductService _productService;
        public CategoryService(IRepository<Category> categoryRepository, IProductService productService) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _productService = productService;
        }

        #region Kategori İşlemleri

        public IQueryable<Category> GetAll() // IQueryable döner
        {
            return _categoryRepository.Query();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> AddAsync(Category category)
        {
            return await _categoryRepository.AddAsync(category);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            return await _categoryRepository.UpdateAsync(category);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<Category> GetByIdAsyncWithSubCategories(int id) // Doğru metot ismi
        {
            return await _categoryRepository.Query() // IQueryable üzerinden sorgu yap
                            .Include(c => c.SubCategories) // Eager loading ile alt kategorileri dahil et
                            .FirstOrDefaultAsync(c => c.Id == id); // İlk bulduğu kaydı getir
        }

        public async Task<CategoryViewModel> GetCategoryWithProductsAsync(int categoryId)
        {
            // Kategori ve bu kategoriye ait ürünleri alacak kodlar
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            var products = await _productService.GetProductsByCategoryIdAsync(categoryId);

            var categoryViewModel = new CategoryViewModel
            {
                CategoryId = category.Id,
                CategoryName = category.ParentCategoryName,
                Products = products.Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToList()
            };

            return categoryViewModel;
        }
        #endregion
    }
}
