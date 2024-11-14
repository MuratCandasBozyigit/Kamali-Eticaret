using ECOMM.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECOMM.Core.ViewModels;

namespace ECOMM.Business.Abstract
{
    public interface ICategoryService : IService<Category>
    {
        Task<CategoryViewModel> GetCategoryWithProductsAsync(int categoryId);
        Task<Category> GetByIdAsyncWithSubCategories(int id); // Doğru isimlendirme
        IQueryable<Category> GetAll();
        // Burada, IService<Category> üzerinden gelen CRUD işlemleri kullanılabilir.
        // Eğer spesifik Category işlemleri olacaksa burada tanımlanabilir.
    }
}
