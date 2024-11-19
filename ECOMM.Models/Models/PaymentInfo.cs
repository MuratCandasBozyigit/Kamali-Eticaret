using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECOMM.Core.Enums;

namespace ECOMM.Core.Models
{
    public class PaymentInfo
    {
        public int PaymentInfoId { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // E.g., Credit Card, PayPal, etc.

        [Required]
        public string PaymentStatus { get; set; } // E.g., Pending, Completed, Failed

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Amount paid

        public string TransactionId { get; set; } // Payment gateway transaction ID

        public PaymentMethodEnum PaymentMethodEnum { get; set; } // Enum türünde
        public PaymentStatus PaymentStatusEnum { get; set; } // Enum türünde
    }
}
