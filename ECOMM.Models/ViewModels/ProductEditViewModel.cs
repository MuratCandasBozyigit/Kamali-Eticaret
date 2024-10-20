using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product{ get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
