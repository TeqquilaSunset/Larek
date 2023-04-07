using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Order.Models;

namespace Order
{
    public class ApplicationContext : DbContext
    {

        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<AllOrder> AllOrders { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=order.db");
        }
    }

}
