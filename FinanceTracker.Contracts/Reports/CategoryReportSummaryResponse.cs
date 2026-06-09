using FinanceTracker.Contracts.Enums;

namespace FinanceTracker.Contracts.Reports;

public record CategoryReportSummaryResponse(
    Guid CategoryId,
    string CategoryName,
    OperationType OperationType,
    decimal TotalAmount,
    int OperationsCount);
