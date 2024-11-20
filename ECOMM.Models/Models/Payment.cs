using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class Payment : BaseModel
    {
        public int OrderId { get; set; }
        public Orders Order { get; set; } // Sipariş ile ilişkilendirildi

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; } // Ödenen miktar

        public string PaymentMethod { get; set; } // Ödeme yöntemi (Kredi Kartı, Havale, vb.)
        public DateTime PaymentDate { get; set; } = DateTime.Now; // Ödeme tarihi
        public string TransactionId { get; set; } // Ödeme işlem ID'si
        public string Status { get; set; } // Ödeme durumu (Başarılı, Başarısız)
    }
}
