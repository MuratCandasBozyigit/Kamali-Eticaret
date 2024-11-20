using Microsoft.EntityFrameworkCore;
using ECOMM.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ECOMM.Data
{
    public class ApplicationDBContext : IdentityDbContext<User, ApplicationRole, string>
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favourites> Favourites { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }      
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItem> OrderItems{ get; set; }
        public DbSet<User>Users {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Orders>()
            .HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>()
           .HasOne(oi => oi.Order)  // OrderItem has one Order
           .WithMany(o => o.OrderItems)  // Orders has many OrderItems
           .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Orders>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);
            // İlişkiler
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany() // Eğer User ile Comment arasında bir koleksiyon ilişkisi yoksa
                .HasForeignKey(c => c.AuthorId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany() // Eğer Product ile Comment arasında bir koleksiyon ilişkisi yoksa
                .HasForeignKey(c => c.ProductId);
            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(aur => new { aur.UserId, aur.RoleId });

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

    }

   
}
