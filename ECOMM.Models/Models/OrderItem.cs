using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class OrderItem
    {
        // Primary Key
        public int OrderItemId { get; set; }

        // Foreign key to Product
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        // Quantity of the product in the order
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // Price of the product at the time of the order
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        // Total cost for this item (Quantity * Price)
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice => Quantity * Price;
    }
}
