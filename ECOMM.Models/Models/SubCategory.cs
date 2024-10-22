using System.Collections.Generic;

namespace ECOMM.Core.Models
{

    public class SubCategory : BaseModel
    {
        public string SubCategoryName { get; set; }

        // Category ile olan ilişkiyi belirtmek için
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
