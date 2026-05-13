namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface ICrudService<TDto>
{
    Task <TDto> GetByIdAsync(Guid id);
    Task<Guid> AddAsync(TDto dto);
    Task UpdateAsync(TDto dto);
    Task RemoveAsync(Guid id);
}