using System.Diagnostics.CodeAnalysis;
using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    [ExcludeFromCodeCoverage]

    public class TaskEntityMapping : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.ToTable("Task");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Title).HasMaxLength(250);

            builder.Property(entity => entity.Description).HasMaxLength(250);

            builder.Property(entity => entity.Priority);

            builder.Property(entity => entity.Status);

            builder.HasOne(x => x.Project)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasMany(x => x.Comments)
                .WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId);


        }
    }
}
