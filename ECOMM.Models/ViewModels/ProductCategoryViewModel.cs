using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class ProductCategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
    }

}
