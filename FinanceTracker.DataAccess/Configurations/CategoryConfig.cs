using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(category => category.Id);

        builder.Property(category => category.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(category => category.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(category => category.OperationType)
            .IsRequired();

        builder.HasOne(category => category.User)
            .WithMany(user => user.Categories)
            .HasForeignKey(category => category.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(category => new { category.UserId, category.Name, category.OperationType })
            .IsUnique();
    }
}
