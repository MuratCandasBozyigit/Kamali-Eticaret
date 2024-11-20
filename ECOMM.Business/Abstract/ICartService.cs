using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Abstract
{
    public interface ICartService
    {
        List<CartItemViewModel> GetCartItems();
        void AddToCart(CartItemViewModel item);
        void RemoveFromCart(int productId);
        void ClearCart();
    }
}
