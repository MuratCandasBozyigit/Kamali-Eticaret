using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string ParentCategoryTag { get; set; }  // Add this line
        public string ParentCategoryDescription { get; set; }  // Add if needed
        public string ProductSize { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }

}
