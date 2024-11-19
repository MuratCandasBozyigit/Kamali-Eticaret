using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class CartViewModel
    {
        public string UserId { get; set; }  // Kullanıcı ID'si
        public List<CartItemViewModel> CartItems { get; set; }  // Sepetteki ürünler
        public decimal TotalPrice
        {
            get
            {
                return CartItems.Sum(item => item.Price * item.Quantity); // Sepet toplam fiyatı hesapla
            }
        }   
    }

}
