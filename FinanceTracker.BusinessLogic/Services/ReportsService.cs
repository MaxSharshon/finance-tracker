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
        var operations = await unitOfWork.FinancialOperations.GetByDateAsync(date, userId);
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
        var operations = await unitOfWork.FinancialOperations.GetByPeriodAsync(startDate, endDate, userId);
        var totalIncome = GetSumForOperationType(operations, OperationType.Income);
        var totalExpenses = GetSumForOperationType(operations, OperationType.Expense);

        return new DatePeriodReportDto
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Operations = mapper.Map<List<FinancialOperationDto>>(operations)
        };
    }

    private static decimal GetSumForOperationType(IEnumerable<FinancialOperation> operations, OperationType type)
    {
        return operations
            .Where(o => o.Category != null && o.Category.OperationType == type)
            .Sum(o => o.Amount);
    }
}