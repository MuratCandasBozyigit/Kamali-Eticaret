using ECOMM.Core.Models;
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
        
            Task<IEnumerable<Product>> GetPaginatedProductsAsync(int page, int pageSize);
        

    }
}
