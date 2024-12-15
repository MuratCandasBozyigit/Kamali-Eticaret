using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class SubCategoryViewModel
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }

      
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }         
        public string CategoryTag { get; set; }        
        public string CategoryDescription { get; set; }


        public string ProductSize { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
