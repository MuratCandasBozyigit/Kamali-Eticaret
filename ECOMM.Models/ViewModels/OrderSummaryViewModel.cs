using System;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; } // Yeni eklenen özellik
        public DateTime OrderDate { get; set; } // Yeni eklenen özellik
        public decimal SubTotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string ShippingAddress { get; set; } // Yeni eklenen özellik
        public string PaymentMethod { get; set; } // Yeni eklenen özellik
        public List<OrderItemViewModel> OrderItems { get; set; } // Yeni eklenen özellik
  
    }
}