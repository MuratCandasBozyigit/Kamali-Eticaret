using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class OrderSummaryViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; } // Sepetteki ürünler
        public decimal SubTotal { get; set; } // Ara toplam
        public decimal Tax { get; set; } // Vergi
        public decimal ShippingCost { get; set; } // Kargo ücreti
        public decimal Total { get; set; } // Toplam tutar
        public string ShippingAddress { get; set; } // Gönderim adresi
        public string PaymentMethod { get; set; } // Ödeme yöntemi
    }
}
