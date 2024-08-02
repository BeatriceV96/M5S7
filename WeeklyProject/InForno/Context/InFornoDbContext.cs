using InForno.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace InForno.Context
{
    public class InFornoDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual  DbSet<Product> Products { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public InFornoDbContext(DbContextOptions<InFornoDbContext> options) : base(options) { }
    }
}
