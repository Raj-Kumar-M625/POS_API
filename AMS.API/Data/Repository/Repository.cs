using ProjectOversight.API.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ProjectOversight.API.Data.Repository;

public abstract class Repository<T> : IRepository<T> where T : class
{
    public Repository(ProjectOversightContext posContext)
    {
        POSContext = posContext;
    }

    protected ProjectOversightContext POSContext { get; set; }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await POSContext.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
    {
        var query = POSContext.Set<T>().Where(expression);
        return await query.AnyAsync() ? await query.ToListAsync() : Enumerable.Empty<T>();
    }
    public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> criteria, Expression<Func<T, object>> expression1)
    {
        return await POSContext.Set<T>().Where(expression).Include(expression1).Include(criteria).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> criteria)
    {
        return await POSContext.Set<T>().Where(expression).Include(criteria).ToListAsync();
    }
    public async Task<IEnumerable<T>> FindByCondition( Expression<Func<T, object>> criteria, Expression<Func<T, object>> expression)
    {
        return await POSContext.Set<T>().Include(expression).Include(criteria).ToListAsync();
    }
    public async Task<T> CreateAsync(T entity)
    {
        var result = await POSContext.Set<T>().AddAsync(entity);
        await POSContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        POSContext.Entry(entity).State = EntityState.Modified;
        await POSContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> DeleteAsync(T entity)
    {
        POSContext.Set<T>().Remove(entity);
        await POSContext.SaveChangesAsync();
        return entity;
    }
    public async Task<T> FindById(Expression<Func<T, bool>> expression)
    {
        var result = await  POSContext.Set<T>().Where(expression).FirstAsync();
        return result;
    }
}