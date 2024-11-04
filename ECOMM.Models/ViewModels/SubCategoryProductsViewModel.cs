using System.Collections.Generic;
using ECOMM.Core.Models;

namespace ECOMM.Web.ViewModels 
{
    public class SubCategoryProductsViewModel
    {
        public SubCategory SubCategory { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
