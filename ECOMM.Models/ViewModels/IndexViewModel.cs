using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class IndexViewModel
    {
        public List<CommentViewModel> Comments { get; set; }
        public List<Product> Products { get; set; } // Ürünlerinizi saklamak için
    }
}
