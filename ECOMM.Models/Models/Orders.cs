using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ECOMM.Core.Enums;

namespace ECOMM.Core.Models
{
  

    public class Orders : BaseModel
    {
        public int OrderId { get; set; }

        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [Required]
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }

        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        public ShippingInfo ShippingInfo { get; set; } // ShippingInfo ile birebir ilişki
        public PaymentInfo PaymentInfo { get; set; } // PaymentInfo ile birebir ilişki
    }
}
