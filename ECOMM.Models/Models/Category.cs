using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.Models
{
    //Listelenen kategorilerden birisi secildiğinde o kategoriye ait olan ürünlerin getirilmesi için sadece
    public class Category: BaseModel
    {

        //Bİre çokluk ve coka coklugu düznele oturup düşünüp ?
        public int CategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string CaegoryName { get; set; }
        public string Description { get; set; }
       
    }
}
