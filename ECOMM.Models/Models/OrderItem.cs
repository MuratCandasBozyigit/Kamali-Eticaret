using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class OrderItem // Sipariş öğesi, her bir ürün için
    {
        public int OrderItemId { get; set; } // Sipariş öğesinin benzersiz kimliği

        public int ProductId { get; set; } // Ürünün benzersiz kimliği

        public Product Product { get; set; } // Ürün nesnesi

        [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den küçük olamaz.")]
        public int Quantity { get; set; } // Sipariş edilen ürün adedi

        [Column(TypeName = "decimal(18,2)")] // Hassasiyet belirleme
        [Range(0.01, double.MaxValue, ErrorMessage = "Ürün fiyatı 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; } // Ürün fiyatı
    }
}
