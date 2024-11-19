using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;

public class PaymentViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public string ShippingAddress { get; set; }
    public string City { get; set; }
    public string PostalCode { get; set; }

    public PaymentMethodEnum PaymentMethod { get; set; }

    public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();
    public decimal TotalAmount { get; set; }
}