
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
        public string ProductTitle { get; set; } 
        public string ProductDescription { get; set; } 
        public float ProductPrice { get; set; } 
        public string ImagePath { get; set; }
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public void UpdateProductInfo(Product product)
        {
            product.ProductTitle = this.ProductTitle;
            product.ProductDescription = this.ProductDescription;
            product.ProductPrice =(float)this.ProductPrice;
            product.CategoryId = this.CategoryId; // Kategori ID'sini güncelle
        }

    }
}
//public IEnumerable<Category> Categories { get; set; }