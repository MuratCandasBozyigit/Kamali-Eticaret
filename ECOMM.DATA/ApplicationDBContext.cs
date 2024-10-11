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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // modelBuilder.Entity<Comment>()
            //.HasOne(c => c.Post)
            //.WithMany(p => p.Comments)
            //.HasForeignKey(c => c.PostId)
            //.OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(aur => new { aur.UserId, aur.RoleId });

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
        }

    }

   
}
