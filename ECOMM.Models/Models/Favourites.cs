using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ECOMM.Core.Models
{
    public class Favourites:BaseModel
    {
      
        public string UserId { get; set; } // Favori ekleyen kullanıcının ID'si
        public virtual User User { get; set; } // Favori ekleyen kullanıcı

        public virtual ICollection<Product> Products { get; set; } // Bir kullanıcının favori ürünleri

        public Favourites()
        {
            Products = new List<Product>();
        }
    }
}

//Bir kullanıcnıın favorilediği ürünleri kgetirmek icin controllerda kullanacgım kod 

//public async Task<IActionResult> Index(string userId)
//{
//    var favourites = await _context.Favourites
//                                   .Include(f => f.Products) // Favori ürünlerle birlikte getir
//                                   .FirstOrDefaultAsync(f => f.UserId == userId);

//    return View(favourites);
//}
