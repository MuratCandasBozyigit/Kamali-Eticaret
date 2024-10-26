
using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; } // Kategori Id'si
        public IEnumerable<Category> Categories { get; set; } // SelectListItem ile kategori bilgisi

        public void UpdateProductInfo(Product product)
        {
            product.ProductTitle = this.ProductTitle;
            product.ProductDescription = this.ProductDescription;
            product.ProductPrice = (float)this.ProductPrice;
            product.ImagePath = this.ImagePath;

            if (product.Category == null)
            {
                product.Category = new Category();
            }

            product.Category.Id = this.CategoryId; // CategoryId üzerinden güncelleme
        }
    }


}
