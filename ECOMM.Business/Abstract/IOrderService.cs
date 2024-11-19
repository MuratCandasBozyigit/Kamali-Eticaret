using ECOMM.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECOMM.Business.Abstract
{
    public interface IOrderService : IService<Orders>
    {
        Task<Orders> CreateAsync(Orders order); // Yeni sipariş oluşturma
        Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(string userId); // Kullanıcıya ait siparişleri alma
    }
}
