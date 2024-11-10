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
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
