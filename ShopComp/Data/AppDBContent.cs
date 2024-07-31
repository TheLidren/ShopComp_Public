using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopComp.Models;

namespace ShopComp.Data
{
    public class AppDBContent : IdentityDbContext<User>
    {
        public AppDBContent(DbContextOptions<AppDBContent> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Tovar> Tovars { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
