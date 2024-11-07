using ECOMM.Core.Models;

namespace ECOMM.Core.ViewModels
{
    public class CartItemViewModel  
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}