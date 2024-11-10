using ECOMM.Business.Abstract;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartSessionKey = "CartItems";

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public List<CartItemViewModel> GetCartItems()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cartItemsJson = session.GetString(CartSessionKey);
        return string.IsNullOrEmpty(cartItemsJson) ? new List<CartItemViewModel>() : JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartItemsJson);
    }

    public void AddToCart(CartItemViewModel item)
    {
        var cartItems = GetCartItems();
        var existingItem = cartItems.FirstOrDefault(i => i.ProductId == item.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity; // Miktarı artır
        }
        else
        {
            cartItems.Add(item); // Yeni ürün ekle
        }

        SaveCartItems(cartItems);
    }

    public void RemoveFromCart(int productId)
    {
        var cartItems = GetCartItems();
        var itemToRemove = cartItems.FirstOrDefault(i => i.ProductId == productId);

        if (itemToRemove != null)
        {
            cartItems.Remove(itemToRemove); // Ürünü kaldır
            SaveCartItems(cartItems);
        }
    }

    public void ClearCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.Remove(CartSessionKey); // Sepeti temizle
    }

    private void SaveCartItems(List<CartItemViewModel> cartItems)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.SetString(CartSessionKey, JsonConvert.SerializeObject(cartItems)); // Sepet öğelerini oturumda sakla
    }
}