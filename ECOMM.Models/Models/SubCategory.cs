using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{

    public class SubCategory : BaseModel
    {
        public string SubCategoryName { get; set; }

        // Category ile olan ilişkiyi belirtmek için
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    
      
    }
}
