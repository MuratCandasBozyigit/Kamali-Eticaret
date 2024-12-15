using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductName { get; set; }
        public string ProductSize { get; set; }

        public decimal Price { get; set; }
        public double? DiscountRate { get; set; }
        [NotMapped]
        public decimal DiscountedPrice => DiscountRate.HasValue
           ? Math.Round(Price - (Price * (decimal)DiscountRate.Value / 100), 2)
           : Price;  
     

        public string ImageUrl { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }


        public string ProductDescription { get; set; }
        public string ParentCategoryName { get; set; }
        public string CategoryName { get; set; }
        public CategoryViewModel Category { get; set; }


        public string SubCategoryName { get; set; }
        public SubCategoryViewModel SubCategory { get; set; }
    }

}
