using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductId { get; set; } // Ürün ID'si
        public string ProductTitle { get; set; } // Ürün Başlığı
        public string ProductDescription { get; set; } // Ürün Açıklaması
        public decimal ProductPrice { get; set; } // Ürün Fiyatı
        public string ImagePath { get; set; } // Resim Yolu
        public int CategoryId { get; set; } // Kategori ID'si
        public IEnumerable<Category> Categories { get; set; } // Kategoriler listesi
    }

}
