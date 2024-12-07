﻿using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductName { get; set; }
        public string ProductSize { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string ProductDescription { get; set; }
        // Kategoriyi ProductViewModel içinde detaylı almak için Category nesnesi ekleyelim
        public string ParentCategoryName { get; set; }
      
        public string CategoryName { get; set; }
        public CategoryViewModel Category { get; set; }  // Burada Category'yi bir nesne olarak ekliyoruz

    
    }

}
