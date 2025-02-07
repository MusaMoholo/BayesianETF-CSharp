using Microsoft.EntityFrameworkCore;
using BayesianETF.Models;

namespace BayesianETF.Services
{
    public class DatabaseContext : DbContext
    {
        public DbSet<StockPrice> StockPrices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=stockdata.db");
        }
    }
}
