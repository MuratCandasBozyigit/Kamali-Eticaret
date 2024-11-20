using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class PaymentViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; } // Yeni eklenen özellik
        public string ShippingCity { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPrice { get; set; } // Yeni eklenen özellik
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingPostalCode { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
    }
}