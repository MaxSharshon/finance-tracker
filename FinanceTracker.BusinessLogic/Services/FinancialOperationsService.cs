using FinanceTracker.Core.Models;
using FinanceTracker.DataAccess.Repositories.Interfaces;

namespace FinanceTracker.BusinessLogic.Services;

public class FinancialOperationsService(IUnitOfWork unitOfWork) : IFinancialOperationsService
{
    public IEnumerable<FinancialOperation> GetAll()
    {
        return unitOfWork.FinancialOperations.GetAll();
    }
}