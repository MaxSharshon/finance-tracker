using AutoMapper;
using FinanceTracker.BusinessLogic.DTOs;
using FinanceTracker.BusinessLogic.DTOs.Reports;
using FinanceTracker.BusinessLogic.Services.Interfaces;
using FinanceTracker.Core.Enums;
using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.BusinessLogic.Services;

public class ReportsService(IUnitOfWork unitOfWork, IMapper mapper) : IReportsService
{
    public async Task<DailyReportDto> GetDailyReportAsync(DateTime date, Guid userId)
    {
        var operations = (await unitOfWork.FinancialOperations.GetByDateAsync(date, userId)).ToList();
        var totalIncome = GetSumForOperationType(operations, OperationType.Income);
        var totalExpenses = GetSumForOperationType(operations, OperationType.Expense);

        return new DailyReportDto
        {
            Date = date,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Operations = mapper.Map<List<FinancialOperationDto>>(operations)
        };
    }

    public async Task<DatePeriodReportDto> GetDatePeriodReportAsync(DateTime startDate, DateTime endDate, Guid userId)
    {
        var operations = (await unitOfWork.FinancialOperations.GetByPeriodAsync(startDate, endDate, userId)).ToList();
        
        var totalIncome = GetSumForOperationType(operations, OperationType.Income);
        var totalExpenses = GetSumForOperationType(operations, OperationType.Expense);

        return new DatePeriodReportDto
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            NetTotal = totalIncome - totalExpenses,
            OperationsCount = operations.Count,
            Operations = mapper.Map<List<FinancialOperationDto>>(operations),
            Categories = GetCategorySummaries(operations),
            Budgets = GetBudgetSummaries(operations)
        };
    }

    private static decimal GetSumForOperationType(IEnumerable<FinancialOperation> operations, OperationType type)
    {
        return operations
            .Where(o => o.Category != null && o.Category.OperationType == type)
            .Sum(o => o.Amount);
    }

    private static IEnumerable<CategoryReportSummaryDto> GetCategorySummaries(
        IEnumerable<FinancialOperation> operations)
    {
        return operations
            .Where(operation => operation.Category is not null)
            .GroupBy(operation => new
            {
                operation.CategoryId,
                operation.Category!.Name,
                operation.Category.OperationType
            })
            .Select(group => new CategoryReportSummaryDto
            {
                CategoryId = group.Key.CategoryId,
                CategoryName = group.Key.Name,
                OperationType = group.Key.OperationType,
                TotalAmount = group.Sum(operation => operation.Amount),
                OperationsCount = group.Count()
            })
            .OrderByDescending(summary => summary.TotalAmount)
            .ToList();
    }

    private static IEnumerable<BudgetReportSummaryDto> GetBudgetSummaries(IEnumerable<FinancialOperation> operations)
    {
        return operations
            .Where(operation => operation.BudgetId.HasValue && operation.Budget is not null)
            .GroupBy(operation => new
            {
                BudgetId = operation.BudgetId!.Value,
                operation.Budget!.Name
            })
            .Select(group =>
            {
                var totalIncome = GetSumForOperationType(group, OperationType.Income);
                var totalExpenses = GetSumForOperationType(group, OperationType.Expense);

                return new BudgetReportSummaryDto
                {
                    BudgetId = group.Key.BudgetId,
                    BudgetName = group.Key.Name,
                    TotalIncome = totalIncome,
                    TotalExpenses = totalExpenses,
                    NetTotal = totalIncome - totalExpenses,
                    OperationsCount = group.Count()
                };
            })
            .OrderByDescending(summary => Math.Abs(summary.NetTotal))
            .ToList();
    }
}
