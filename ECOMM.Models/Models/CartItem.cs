namespace ECOMM.Core.Models
{
    public class CartItem:BaseModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
