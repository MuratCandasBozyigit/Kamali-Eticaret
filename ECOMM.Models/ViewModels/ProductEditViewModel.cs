
using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string ImagePath { get; set; }
        public string CategoryName { get; set; } // Kategori adı
        public IEnumerable<string> Categories { get; set; } // Kategori adlarını içeren liste

        public void UpdateProductInfo(Product product)
        {
            product.ProductTitle = this.ProductTitle;
            product.ProductDescription = this.ProductDescription;
            product.ProductPrice = (float)this.ProductPrice;
            product.Category.ParentCategoryName = this.CategoryName; // Kategori adını güncelle
            product.ImagePath = this.ImagePath;
        }
    }

}
