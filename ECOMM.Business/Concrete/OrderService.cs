﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<Orders> CreateAsync(Orders order)
        {
            return await AddAsync(order); // Yeni siparişi ekle
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUserIdAsync(string userId)
        {
            // Kullanıcıya ait siparişleri almak için repository'den filtreleme yapmalısınız.
            // Burada örnek bir filtreleme yapıyoruz. Gerçek uygulamanızda kendi veri erişim mantığınıza göre düzenleyin.
            var orders = await GetAllAsync();
            return orders.Where(o => o.UserId == userId); // Kullanıcı kimliğine göre filtrele
        }
    }
}
