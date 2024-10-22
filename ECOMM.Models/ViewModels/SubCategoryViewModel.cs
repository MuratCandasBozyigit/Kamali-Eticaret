using ECOMM.Core.Models;
using System.Collections.Generic;

namespace ECOMM.Core.ViewModels
{
    public class SubCategoryViewModel
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }

        // Category ile ilgili alanlar:
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }          // Ana kategori adı
        public string CategoryTag { get; set; }           // Ana kategori etiketi
        public string CategoryDescription { get; set; }   // Ana kategori açıklaması
    }




}
