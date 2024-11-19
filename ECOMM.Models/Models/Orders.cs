using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECOMM.Core.Enums;

namespace ECOMM.Core.Models
{
    public class Orders : BaseModel
    {
        // Primary Key
        public int OrderId { get; set; }

        // User ID for referencing the customer who made the order
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }

        // List of order items (products in the order)
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Order Date
        [Required]
        public DateTime OrderDate { get; set; }

        // Total amount for the order (with a decimal type for currency)
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }

        // Order status (e.g., Pending, Shipped, Delivered)
        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        // Shipping information (address, etc.)
        public ShippingInfo ShippingInfo { get; set; }

        // Payment information (payment method, transaction ID, etc.)
        public PaymentInfo PaymentInfo { get; set; }

        // Additional order-related fields (like coupon codes, notes, etc.)
        public string CouponCode { get; set; }
        public string Notes { get; set; }
    }
}
