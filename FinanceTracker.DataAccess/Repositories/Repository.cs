using System.Linq.Expressions;
using FinanceTracker.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.DataAccess.Repositories;

public class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext Context = context;

    public TEntity? Get(Guid id)
    {
        return Context.Set<TEntity>().Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
        return Context.Set<TEntity>().ToList();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Where(predicate);
    }

    public void Add(TEntity entity)
    {
        Context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
    }
}