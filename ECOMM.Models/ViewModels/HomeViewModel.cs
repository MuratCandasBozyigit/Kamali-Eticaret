using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace ECOMM.Core.ViewModels
    {
        public class HomeViewModel
        {
            public List<Product> Products { get; set; }
            public List<Category> Categories { get; set; }
            public int TotalCount { get; set; }
            public int PageSize { get; set; } = 6; // Sayfada gösterilecek ürün sayısı
            public int CurrentPage { get; set; } = 1; // Mevcut sayfa
            public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        }
    }

