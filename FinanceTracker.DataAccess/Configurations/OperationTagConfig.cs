using FinanceTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceTracker.DataAccess.Configurations;

public class OperationTagConfig : IEntityTypeConfiguration<OperationTag>
{
    public void Configure(EntityTypeBuilder<OperationTag> builder)
    {
        builder.HasKey(operationTag => new { operationTag.FinancialOperationId, operationTag.TagId });

        builder.HasOne(operationTag => operationTag.FinancialOperation)
            .WithMany(operation => operation.OperationTags)
            .HasForeignKey(operationTag => operationTag.FinancialOperationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(operationTag => operationTag.Tag)
            .WithMany(tag => tag.OperationTags)
            .HasForeignKey(operationTag => operationTag.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
