using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;

namespace ECOMM.Business.Abstract
{
    public interface IProductService :IService<Product>
    {

        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<List<ProductViewModel>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetPaginatedProductsAsync(int page, int pageSize);
        Task<IEnumerable<Product>> GetBySubCategoryIdAsync(int subCategoryId);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<List<ProductViewModel>> GetProductsByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsRandom(int page, int pageSize);

    }
}
