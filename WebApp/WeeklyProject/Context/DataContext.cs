using Microsoft.EntityFrameworkCore;
using WeeklyProject.Models;

namespace WeeklyProject.Context
{
    public class DataContext: DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }

        public DataContext(DbContextOptions<DataContext> opt): base(opt)
        {
        }
    }
}
