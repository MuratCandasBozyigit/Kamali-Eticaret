
using ECOMM.Core.Models;

public class Comment : BaseModel
{
    public string? Content { get; set; }
    public string AuthorId { get; set; }
    public User Author { get; set; }
    public DateTime DateCommented { get; set; } = DateTime.Now;

    public int ProductId { get; set; }
    public Product Product { get; set; }
}