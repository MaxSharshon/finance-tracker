using FinanceTracker.Contracts.Enums;
using FinanceTracker.Contracts.FinancialOperations;

namespace FinanceTracker.UI.Services.Interfaces;

public interface IFinancialOperationService : IService<FinancialOperationRequest, FinancialOperationResponse>
{
    Task<IEnumerable<FinancialOperationResponse>> GetAllAsync(
        DateTime? startDate,
        DateTime? endDate,
        Guid? categoryId,
        Guid? budgetId,
        OperationType? operationType);
}
