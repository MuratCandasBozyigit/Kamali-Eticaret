using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;

namespace ECOMM.Core.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }  // List of cart items
        public decimal TotalAmount { get; set; }                // Total amount of the cart
        public User User { get; set; }                           // User details
    }
}
