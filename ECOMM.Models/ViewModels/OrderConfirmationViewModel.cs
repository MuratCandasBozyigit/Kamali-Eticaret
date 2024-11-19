using System;
using System.Collections.Generic;
using ECOMM.Core.Models;

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
        public List<OrderItem> OrderItems { get; set; } // Sipariş öğeleri
    }

    // Sipariş öğesi
    public class OrderItem
    {
        public int ProductId { get; set; }              // Ürün ID'si
        public string ProductName { get; set; }         // Ürün adı
        public int Quantity { get; set; }               // Adet
        public decimal Price { get; set; }              // Ürün fiyatı
        public decimal TotalPrice { get; set; }         // Toplam fiyat (Price * Quantity)
    }
}
