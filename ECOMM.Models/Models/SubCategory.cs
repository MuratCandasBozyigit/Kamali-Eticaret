using System.Collections.Generic;

namespace ECOMM.Core.Models
{
    public class SubCategory : BaseModel
    {
        public int Id { get; set; } // Anahtar alan
        public string SubCatName { get; set; }
        public string ParentCategoryDescription { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
