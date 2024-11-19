using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public enum OrderStatus
    {
        Pending,
        Shipped,
        Delivered,
        Canceled
    }

    public enum PaymentMethodEnum
    {
        CreditCard,
        BankTransfer,
        CashOnDelivery
    }

    public class Orders : BaseModel
    {
        public int OrderId { get; set; } // Sipariş Kimliği

        [Required]
        public string UserId { get; set; } // Kullanıcı kimliği (User sınıfındaki Id ile uyumlu)

        public User User { get; set; } // Kullanıcı nesnesi

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Siparişin içindeki ürünler
        public DateTime OrderDate { get; set; } // Sipariş tarihi

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Toplam tutar 0'dan büyük olmalıdır.")]
        public decimal TotalAmount { get; set; } // Toplam tutar

        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; } // Sipariş durumu (örn. 'Pending', 'Shipped', 'Delivered')

        [Required]
        [StringLength(500)]
        public string ShippingAddress { get; set; } // Gönderim adresi

        [Required]
        [EnumDataType(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum PaymentMethod { get; set; } // Ödeme yöntemi (örneğin 'Credit Card', 'PayPal' vb.)
    }
}
