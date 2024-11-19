namespace ECOMM.Core.ViewModels
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }          // Ürünün ID'si
        public int Quantity { get; set; }           // Ürün adedi
        public decimal Price { get; set; }          // Ürün fiyatı
        public string Name { get; set; }            // Ürün adı
    }
}