using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.Models
{
   
        public class OrderItem // Sipariş öğesi, her bir ürün için
        {
            public int OrderItemId { get; set; } // Sipariş öğesinin benzersiz kimliği
            public int ProductId { get; set; } // Ürünün benzersiz kimliği
            public Product Product { get; set; } // Ürün nesnesi
            public int Quantity { get; set; } // Sipariş edilen ürün adedi

            [Column(TypeName = "decimal(18,2)")] // Hassasiyet belirleme
            public decimal Price { get; set; } // Ürün fiyatı
        }
    
}
