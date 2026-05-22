namespace FinanceTracker.BusinessLogic.Services.Interfaces;

public interface IScopedCrudService<TDto, in TScope>
{
    Task <TDto> GetByIdAsync(Guid id, TScope scope);
    Task<Guid> AddAsync(TDto dto, TScope scope);
    Task UpdateAsync(TDto dto, TScope scope);
    Task RemoveAsync(Guid id, TScope scope);
}
