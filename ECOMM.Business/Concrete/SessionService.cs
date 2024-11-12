using ECOMM.Business.Abstract;
using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartSessionKey = "CartItems";
    private const string FavouritesSessionKey = "FavouritesItems"; // Favoriler için oturum anahtarı

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #region Cart Methods

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

    #endregion

    #region Favourites Methods

    public List<CartItemViewModel> GetFavouritesItems()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var favouritesItemsJson = session.GetString(FavouritesSessionKey);
        return string.IsNullOrEmpty(favouritesItemsJson) ? new List<CartItemViewModel>() : JsonConvert.DeserializeObject<List<CartItemViewModel>>(favouritesItemsJson);
    }

    public void AddToFavourites(CartItemViewModel item)
    {
        var favouritesItems = GetFavouritesItems();
        var existingItem = favouritesItems.FirstOrDefault(i => i.ProductId == item.ProductId);

        if (existingItem == null)
        {
            favouritesItems.Add(item); // Yeni ürün favorilere ekle
        }

        SaveFavouritesItems(favouritesItems);
    }

    public void RemoveFromFavourites(int productId)
    {
        var favouritesItems = GetFavouritesItems();
        var itemToRemove = favouritesItems.FirstOrDefault(i => i.ProductId == productId);

        if (itemToRemove != null)
        {
            favouritesItems.Remove(itemToRemove); // Ürünü favorilerden kaldır
            SaveFavouritesItems(favouritesItems);
        }
    }

    public void ClearFavourites()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.Remove(FavouritesSessionKey); // Favorileri temizle
    }

    #endregion

    #region Private Methods

    private void SaveCartItems(List<CartItemViewModel> cartItems)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.SetString(CartSessionKey, JsonConvert.SerializeObject(cartItems)); // Sepet öğelerini oturumda sakla
    }

    private void SaveFavouritesItems(List<CartItemViewModel> favouritesItems)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.SetString(FavouritesSessionKey, JsonConvert.SerializeObject(favouritesItems)); // Favori öğelerini oturumda sakla
    }

    #endregion
}
