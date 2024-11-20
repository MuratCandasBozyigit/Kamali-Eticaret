using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class Orders : BaseModel
    {
        public int OrderId { get; set; }
        public int OrderNumber {  get; set; }

        public string UserId { get; set; } // User sınıfındaki Id ile uyumlu hale getirildi
        public User User { get; set; } // Kullanıcı nesnesi

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Siparişin içindeki ürünler
        public DateTime OrderDate { get; set; } // Sipariş tarihi

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } // Toplam tutar

        public string Status { get; set; } // Sipariş durumu (örn. 'Pending', 'Shipped', 'Delivered')
        public string ShippingAddress { get; set; } // Gönderim adresi
        public string PaymentMethod { get; set; } // Ödeme yöntemi
    }
}
