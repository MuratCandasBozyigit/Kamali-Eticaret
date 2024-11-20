﻿using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Abstract
{
    public interface IOrderService:IService<Orders>
    {
        Task PlaceOrderAsync(Orders order);
        Task<List<Orders>> GetOrdersByUserAsync(string userId);
        void PlaceOrder(Orders order);
    }
}
