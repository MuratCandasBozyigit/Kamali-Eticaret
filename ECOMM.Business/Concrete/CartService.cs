using ECOMM.Business.Abstract;
using ECOMM.Core.ViewModels;

namespace ECOMM.Business.Concrete
{
    public class CartService : ICartService
    {
        private readonly List<CartItemViewModel> _cartItems = new List<CartItemViewModel>();

        public List<CartItemViewModel> GetCartItems()
        {
            return _cartItems;
        }

        public void AddToCart(CartItemViewModel item)
        {
            var existingItem = _cartItems.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                _cartItems.Add(item);
            }
        }

        public void RemoveFromCart(int productId)
        {
            var item = _cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                _cartItems.Remove(item);
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}
