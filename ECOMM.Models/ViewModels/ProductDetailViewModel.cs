using ECOMM.Core.Models;

namespace ECOMM.Core.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; }
        public List<ProductViewModel> RelatedProducts { get; set; }
    }
}
