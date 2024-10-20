using System.Collections.Generic;

namespace ECOMM.Core.Models
{
    public class Product:BaseModel
    {
       
        public string ProductTitle { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
        public string? ImagePath { get; set; }

        public virtual ICollection<Favourites> FavouritedBy { get; set; } // Ürünü favorilerine ekleyen kullanıcılar

        public Product()
        {
            FavouritedBy = new List<Favourites>();
        }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}


