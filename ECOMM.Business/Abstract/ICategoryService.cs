using ECOMM.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ECOMM.Business.Abstract
{
    public interface ICategoryService : IService<Category>
    {
        // Burada, IService<Category> üzerinden gelen CRUD işlemleri kullanılabilir.
        // Eğer spesifik Category işlemleri olacaksa burada tanımlanabilir.
    }
}
