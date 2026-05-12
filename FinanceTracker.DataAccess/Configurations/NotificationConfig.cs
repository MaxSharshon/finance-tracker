using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(notification => notification.Id);

        builder.Property(notification => notification.Id)
            .HasDefaultValueSql("newid()");

        builder.Property(notification => notification.Message)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(notification => notification.CreatedAt)
            .IsRequired();

        builder.HasOne(notification => notification.User)
            .WithMany(user => user.Notifications)
            .HasForeignKey(notification => notification.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
