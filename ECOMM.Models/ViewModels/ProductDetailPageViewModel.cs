using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class ProductDetailPageViewModel
    {
        public ProductViewModel Product { get; set; }  // Ana ürün bilgisi
        public List<ProductViewModel> RelatedProducts { get; set; }  // İlgili ürünler listesi
        public List<CommentViewModel> Comments { get; set; }  // Yorumlar listesi
        public List<Product> Products { get; set; }
        public ProductDetailPageViewModel()
        {
            RelatedProducts = new List<ProductViewModel>();
            Comments = new List<CommentViewModel>();
        }
    }

}
