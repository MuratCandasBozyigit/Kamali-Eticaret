using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;

namespace ECOMM.Business.Concrete
{
    public class OrderService : Service<Orders>, IOrderService
    {
        private readonly IRepository<Orders> _orderRepository;

        // Burada IRepository<Orders> tipinde bir parametre alıyoruz
        public OrderService(IRepository<Orders> orderRepository) : base(orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Orders> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Orders>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Orders> AddAsync(Orders orders)
        {
            return await _orderRepository.AddAsync(orders);
        }

        public async Task<Orders> UpdateAsync(Orders orders)
        {
            return await _orderRepository.UpdateAsync(orders);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }
    }
}
