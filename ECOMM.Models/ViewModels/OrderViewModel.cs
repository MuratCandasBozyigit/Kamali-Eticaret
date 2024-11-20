namespace ECOMM.Core.ViewModels
{
    public class OrderViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
    }
}
