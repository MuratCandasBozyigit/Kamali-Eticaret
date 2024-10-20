using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.Models
{
    public class SubCategory:BaseModel
    {
        public string SubCatName { get; set; }
        public string ParentCategoryDescription { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
