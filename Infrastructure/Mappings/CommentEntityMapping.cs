using System.Diagnostics.CodeAnalysis;
using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    [ExcludeFromCodeCoverage]

    public class CommentEntityMapping :  IEntityTypeConfiguration<CommentsEntity>
    {
        public void Configure(EntityTypeBuilder<CommentsEntity> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Comment).HasMaxLength(250);


            builder.HasOne(x => x.Task)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.TaskId);


        }
    }
}
