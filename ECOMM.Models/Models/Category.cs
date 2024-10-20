using ECOMM.Core.Models;

public class Category : BaseModel
{
    public int? ParentCategoryId { get; set; }  // Opsiyonel ana kategori ID
    public string ParentCategoryName { get; set; }  // Ana kategori adı
    public string ParentCategoryTag { get; set; }    // Ana kategori etiketi
    public string ParentCategoryDescription { get; set; }  // Ana kategori açıklaması

    // Alt kategoriler
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
