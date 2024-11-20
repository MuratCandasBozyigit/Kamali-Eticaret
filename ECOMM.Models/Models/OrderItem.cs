using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class OrderItem // Sipariş öğesi, her bir ürün için
    {
        public int OrderItemId { get; set; } // Sipariş öğesinin benzersiz kimliği
        public int ProductId { get; set; } // Ürünün benzersiz kimliği
        public Product Product { get; set; } // Ürün nesnesi
        public int Quantity { get; set; } // Sipariş edilen ürün adedi

        [Column(TypeName = "decimal(18,2)")] // Hassasiyet belirleme
        public decimal Price { get; set; } // Ürün fiyatı

        public int OrderId { get; set; } // Siparişin kimliği
        public Orders Order { get; set; } // İlgili sipariş

        public decimal UnitPrice { get; set; } // Ürün başına fiyat

        // Yeni eklenen özellikler
        public string ProductName { get; set; } // Ürün ismi
        public string ImagePath { get; set; } // Ürün resmi yolu

        public decimal TotalPrice => Quantity * UnitPrice; // Toplam fiyat: Miktar * Birim fiyat
    }
}
