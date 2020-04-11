using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiPractice.Models
{
    public class ShopContext :DbContext
    {

        public ShopContext(DbContextOptions<ShopContext> options):base(options){}



        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Category>().HasMany(s => s.Products).WithOne(c => c.Category).HasForeignKey(c => c.CategoryId);

            model.Entity<Order>().HasMany(c => c.Products);

            model.Entity<Order>().HasOne(u => u.User);

            model.Entity<User>().HasMany(o => o.Orders).WithOne(u => u.User).HasForeignKey(s => s.UserId);

            model.Seed();
        }
        public DbSet<Product>Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
