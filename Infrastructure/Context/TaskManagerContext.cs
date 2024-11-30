using System.Diagnostics.CodeAnalysis;
using Domain.Domain;
using Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagerContext).Assembly);

            var entityTypes = modelBuilder.Model
                                                        .GetEntityTypes()
                                                        .Where(t => typeof(Entity).IsAssignableFrom(t.ClrType));

            foreach (var entityType in entityTypes)
            {
                var configurationType = typeof(EntityMapping<>)
                    .MakeGenericType(entityType.ClrType);

                modelBuilder
                    .ApplyConfiguration((dynamic)Activator.CreateInstance(configurationType));
            }


        }
        public DbSet<ProjectEntity> Project { get; set; }

        public DbSet<TaskEntity> Task { get; set; }

        public DbSet<AuditLogEntity> AuditLog { get; set; }



        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries().Where(entity => entity.Entity.GetType().GetProperty("CreatedAt") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                    continue;

                entry.Property("LastModified").CurrentValue = DateTime.Now;
                entry.Property("CreatedAt").IsModified = false;

            }

            return base.SaveChangesAsync(cancellationToken);

        }
    }
}
