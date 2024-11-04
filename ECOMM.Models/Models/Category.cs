using ECOMM.Core.Models;

namespace ECOMM.Core.Models
{
    public class Category : BaseModel
    {
        public string ParentCategoryName { get; set; }
        public string ParentCategoryTag { get; set; }
        public string ParentCategoryDescription { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
