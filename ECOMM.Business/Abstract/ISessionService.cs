using ECOMM.Core.ViewModels; // CartItemViewModel burada tanımlıysa

namespace ECOMM.Business.Abstract
{
    public interface ISessionService
    {
        List<CartItemViewModel> GetCartItems(); // Oturumdan sepet öğelerini al
        void AddToCart(CartItemViewModel item); // Sepete ürün ekle
        void RemoveFromCart(int productId); // Sepetten ürün kaldır
        void ClearCart(); // Sepeti temizle
    }
}