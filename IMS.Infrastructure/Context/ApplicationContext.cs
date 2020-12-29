using IMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IMS.Infrastructure.Context
{
    public class ApplicationContext : DbContext
    {
        private readonly string DefaultSchema = "ims";

        public ApplicationContext() { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Change schema from all entities
            modelBuilder.HasDefaultSchema(DefaultSchema);

            // Fix the auto generated key name
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Truck> Truck { get; set; }
    }
}
