using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class Product : BaseModel
    {
        public string ProductTitle { get; set; }

        [MaxLength(11, ErrorMessage = "Ürün adı en fazla 11 karakter olmalıdır.")]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        [Column(TypeName = "decimal(18,2)")] // Hassasiyeti belirleme
        public decimal ProductPrice { get; set; } // Ürün fiyatı için decimal kullanıyoruz

        public string? ImagePath { get; set; }

        public virtual ICollection<Favourites> FavouritedBy { get; set; } // Ürünü favorilerine ekleyen kullanıcılar

        public Product()
        {
            FavouritedBy = new List<Favourites>();
        }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

       //public ICollection<Category> Categories { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
