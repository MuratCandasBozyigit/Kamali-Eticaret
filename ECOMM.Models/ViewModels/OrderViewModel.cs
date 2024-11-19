using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class OrderViewModel
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }  // Sepet öğeleri
    }

}
