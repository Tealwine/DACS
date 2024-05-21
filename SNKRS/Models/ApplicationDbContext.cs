using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SNKRS.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<ProductGallery> ProductGalleries { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Portfolio>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .Map(pc =>
                {
                    pc.MapLeftKey("ProductId");
                    pc.MapRightKey("CategoryId");
                    pc.ToTable("ProductCategory");
                });

            modelBuilder.Entity<Portfolio>()
                .HasMany(p => p.Reviews)
                .WithRequired(r => r.Product)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Portfolio>()
                .Property(e => e.Description);

            modelBuilder.Entity<Portfolio>()
                .HasMany(e => e.ProductGalleries)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.ProductId);

       

        

            base.OnModelCreating(modelBuilder);
        }
    }
}
