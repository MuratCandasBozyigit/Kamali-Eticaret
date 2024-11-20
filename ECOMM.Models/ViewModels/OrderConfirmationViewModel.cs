using System;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public int OrderId { get; set; }               // Sipariş ID'si
        public DateTime OrderDate { get; set; }         // Sipariş tarihi
        public decimal TotalAmount { get; set; }        // Toplam ödeme miktarı
        public string PaymentMethod { get; set; }       // Ödeme yöntemi (örneğin: Kredi Kartı, PayPal)
        public string PaymentStatus { get; set; }       // Ödeme durumu (örneğin: Success, Pending)

        // Siparişin içeriği
        public List<OrderItemViewModel> OrderItems { get; set; } // Sipariş öğeleri
    }

}