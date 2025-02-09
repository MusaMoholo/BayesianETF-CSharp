using Microsoft.EntityFrameworkCore;
using BayesianETF.Models;

namespace BayesianETF
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ModelPrediction> ModelPredictions { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ModelPrediction>().ToTable("ModelPredictions");
            base.OnModelCreating(modelBuilder);
        }
    }
}
