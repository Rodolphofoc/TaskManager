using System.Diagnostics.CodeAnalysis;
using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    [ExcludeFromCodeCoverage]

    public class ProjectEntityMapping : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.ToTable("Project");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Name).HasMaxLength(250);

            builder.Property(entity => entity.Description).HasMaxLength(250);



            builder.HasMany(x => x.Tasks)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectId);


        }

    }
}
