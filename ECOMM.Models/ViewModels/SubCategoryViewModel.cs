using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class SubCategoryViewModel
    {
        public int Id { get; set; } // BaseModel'den gelen Id
        public string SubCategoryName { get; set; }
        public int CategoryId { get; set; } // İlişkilendirilmiş kategori
        public string CategoryName { get; set; } // Ana kategori adı
        public List<Category> Categories { get; set; } // Kategori listesi
    }
    


}
