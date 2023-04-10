using Delivery.Models;
using Microsoft.EntityFrameworkCore;

namespace Delivery
{
    public class ApplicationContext : DbContext
    {
        public DbSet<CollectedOrders> CollectedOrders { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=delivery.db");
        }
    }
}
