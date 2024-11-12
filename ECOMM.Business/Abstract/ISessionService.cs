using ECOMM.Core.ViewModels; // CartItemViewModel burada tanımlıysa

namespace ECOMM.Business.Abstract
{
    public interface ISessionService
    {
        List<CartItemViewModel> GetCartItems(); // Oturumdan sepet öğelerini al
        void AddToCart(CartItemViewModel item); // Sepete ürün ekle
        void RemoveFromCart(int productId); // Sepetten ürün kaldır
        void ClearCart(); // Sepeti
                          // 
        List<CartItemViewModel> GetFavouritesItems(); // Oturumdan sepet öğelerini al
        void AddToFavourites(CartItemViewModel item); // Sepete ürün ekle
        void RemoveFromFavourites(int productId); // Sepetten ürün kaldır
        void ClearFavourites(); // Sepeti temizle
    }
}