using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class Product : BaseModel
    {
        [Required, MaxLength(100)]
        public string ProductTitle { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        public double? DiscountRate { get; set; } // Yüzde indirim oranı (opsiyonel)

        [NotMapped]
        public decimal DiscountedPrice => DiscountRate.HasValue
            ? Math.Round(ProductPrice - (ProductPrice * (decimal)DiscountRate.Value / 100), 2)
            : ProductPrice;

     

        [Required]
        public string ProductDescription { get; set; }

        public string ImagePath { get; set; } // Ürün görsel yolu
        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; } // Kategori ilişkisi

        [Required]
        public List<string> ProductSizes { get; set; } = new List<string>(); // Ürün beden bilgisi

        public int StockQuantity { get; set; } // Toplam stok bilgisi

        [NotMapped]
        public Dictionary<string, int> SizeStock { get; set; } = new Dictionary<string, int>(); // Beden bazlı stok bilgisi
    }
}
