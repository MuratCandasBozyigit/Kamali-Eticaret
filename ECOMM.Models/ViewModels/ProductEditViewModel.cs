
using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; } // Ürün ID'si
        public string ProductTitle { get; set; } // Ürün Başlığı
        public string ProductDescription { get; set; } // Ürün Açıklaması
        public decimal ProductPrice { get; set; } // Ürün Fiyatı
        public string ImagePath { get; set; } // Resim Yolu
        public int CategoryId { get; set; } // Kategori ID'si
        public IEnumerable<Category> Categories { get; set; }
        // Metod: Ürün bilgilerini güncelle
        public void UpdateProductInfo(Product product)
        {
            product.ProductTitle = this.ProductTitle;
            product.ProductDescription = this.ProductDescription;
            product.ProductPrice = (float)this.ProductPrice; // Dönüştürme yapıyoruz
            product.CategoryId = this.CategoryId;
            product.ImagePath = this.ImagePath;
        }
    }
}
