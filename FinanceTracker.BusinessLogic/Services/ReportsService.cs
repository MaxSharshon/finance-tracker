using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Enums;
using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.BusinessLogic.Services;

public class ReportsService(IUnitOfWork unitOfWork, IMapper mapper) : IReportsService
{
    public async Task<DailyReportDto> GetDailyReportAsync(DateTime date)
    {
        var operations = (await unitOfWork.FinancialOperations.GetByDateWithBalanceChangeAsync(date)).ToList();

        var totalIncome = operations
            .Where(o => o.BalanceChange!.OperationType == OperationType.Income)
            .Sum(o => o.BalanceChange!.Amount);
        
        var totalExpenses = operations
            .Where(o => o.BalanceChange!.OperationType == OperationType.Expense)
            .Sum(o => o.BalanceChange!.Amount);

        return new DailyReportDto
        {
            Date = date,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Operations = mapper.Map<List<FinancialOperationDto>>(operations)
        };
    }
}