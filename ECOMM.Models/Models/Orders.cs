using System;
using System.Collections.Generic;

namespace ECOMM.Core.Models
{
    public class Orders : BaseModel
    {
        public int OrderId { get; set; } // Siparişin benzersiz kimliği
        public int UserId { get; set; } // Kullanıcının benzersiz kimliği
        public User User { get; set; } // Kullanıcı nesnesi
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Siparişin içindeki ürünler
        public DateTime OrderDate { get; set; } // Sipariş tarihi
        public decimal TotalAmount { get; set; } // Toplam tutar
        public string Status { get; set; } // Sipariş durumu (örn. 'Pending', 'Shipped', 'Delivered')

        // İsteğe bağlı olarak, ek bilgiler de ekleyebilirsiniz
        public string ShippingAddress { get; set; } // Gönderim adresi
        public string PaymentMethod { get; set; } // Ödeme yöntemi
    }

    public class OrderItem // Sipariş öğesi, her bir ürün için
    {
        public int OrderItemId { get; set; } // Sipariş öğesinin benzersiz kimliği
        public int ProductId { get; set; } // Ürünün benzersiz kimliği
        public Product Product { get; set; } // Ürün nesnesi
        public int Quantity { get; set; } // Sipariş edilen ürün adedi
        public decimal Price { get; set; } // Ürün fiyatı
    }

    // User ve Product sınıflarını başka bir dosyadan kullanın
    // Bu sınıfları tekrar tanımlamaktan kaçının.
}
