using ECOMM.Core.Models;

public class Category : BaseModel
{
    public int ParentCategoryId { get; set; }  // Bu alan, Id olarak adlandırılmalı.
    public string ParentCategoryName { get; set; }
    public string ParentCategoryTag { get; set; }
    public string ParentCategoryDescription { get; set; }



    // Alt kategoriler
    public virtual int? Id { get; set; }
    public virtual SubCategory SubCategories { get; set; }

}
