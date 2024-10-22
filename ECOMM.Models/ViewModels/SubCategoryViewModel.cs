using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
   
        public class SubCategoryViewModel
        {
            public int Id { get; set; }
            public string SubCategoryName { get; set; }
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }

            // Eklediğimiz Categories özelliği
          //  public IEnumerable<CategoryViewModel> Categories { get; set; }
        }




}
