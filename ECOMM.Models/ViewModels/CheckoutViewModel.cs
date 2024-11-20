using ECOMM.Core.Models;

namespace ECOMM.Core.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel>? CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }




        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool SameAddress { get; set; }
        public bool SaveInfo { get; set; }
        public string PromoCode { get; set; }
    }
}
