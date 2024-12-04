
using ECOMM.Core.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ECOMM.Core.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
      public string ProductName { get; set; }
        public string ProductTitle { get; set; } 
        public string ProductDescription { get; set; } 
        public decimal ProductPrice { get; set; }
        public List<string> ProductSizes { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public void UpdateProductInfo(Product product)
        {
            product.ProductSizes = this.ProductSizes;
            product.ProductName = this.ProductName;
            product.ProductTitle = this.ProductTitle;
            product.ProductDescription = this.ProductDescription;
            product.ProductPrice =(decimal)this.ProductPrice;
            product.CategoryId = this.CategoryId; // Kategori ID'sini güncelle
        }

    }
}