using ECOMM.Core.Models;

namespace ECOMM.Core.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity; // Ürün başına toplam fiyat

        // Add the UnitPrice property
        public decimal UnitPrice { get; set; } // Can be same as Price or different if needed

        // Add the Product property to store complete product details if needed
        public Product Product { get; set; }
    }
}
