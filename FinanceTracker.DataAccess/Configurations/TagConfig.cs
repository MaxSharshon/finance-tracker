using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class TagConfig : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(tag => tag.Id);

        builder.Property(tag => tag.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(tag => tag.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasOne(tag => tag.User)
            .WithMany()
            .HasForeignKey(tag => tag.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tag => new { tag.UserId, tag.Name })
            .IsUnique();
    }
}
