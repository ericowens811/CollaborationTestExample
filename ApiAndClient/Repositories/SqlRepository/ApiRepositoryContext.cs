using ApiAndClient.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiAndClient.Repositories.SqlRepository
{
    public class ApiRepositoryContext : DbContext
    {
        public DbSet<ResourceEntity> Resources { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }

        public ApiRepositoryContext(DbContextOptions<ApiRepositoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ResourceEntity>()
                .HasMany(e => e.Messages)
                .WithOne(e => e.Resource)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder
                .Entity<ResourceEntity>()
                .HasIndex(i => i.ResourceId)
                .IsUnique();
        }
    }
}
