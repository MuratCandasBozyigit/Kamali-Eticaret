

namespace ECOMM.Core.Models
{ 

public class Comment : BaseModel
{
    public string? Content { get; set; } // Yorum içeriği
    public string? AuthorId { get; set; } // Yorumu yapan kullanıcının ID'si
    public User Author { get; set; } // Kullanıcı ilişkisi
    public string? AuthorName { get; set; } // Kullanıcının adı veya kullanıcı adı
    public DateTime DateCommented { get; set; } = DateTime.Now; // Yorum tarihi
    public bool IsApproved { get; set; } = false; // Onay durumu

    public int ProductId { get; set; } // Yorumun ait olduğu ürünün ID'si
    public Product Product { get; set; } // Ürün ilişkisi
}
}