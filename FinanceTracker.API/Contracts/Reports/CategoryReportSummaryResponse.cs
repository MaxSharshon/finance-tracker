using FinanceTracker.Core.Enums;

namespace FinanceTracker.API.Contracts.Reports;

public record CategoryReportSummaryResponse(
    Guid CategoryId,
    string CategoryName,
    OperationType OperationType,
    decimal TotalAmount,
    int OperationCount);
