using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<List<ProductViewModel>> GetProductsSortedAsync(string sortOrder);
    }
}
