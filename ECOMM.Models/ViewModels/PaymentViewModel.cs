using System.Collections.Generic;
using ECOMM.Core.Enums;

namespace ECOMM.Core.ViewModels
{
    public class PaymentViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ShippingCity { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingPostalCode { get; set; }
        public string UserId { get; set; }
    }


}
