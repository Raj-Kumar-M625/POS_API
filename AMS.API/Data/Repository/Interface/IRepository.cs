using System.Linq.Expressions;

namespace ProjectOversight.API.Data.Repository.Interface;

public interface IRepository<T>
{
    Task<IEnumerable<T>> FindAllAsync();
    Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> cretiria, Expression<Func<T, object>> expression1);
    Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>> cretiria);
    Task<IEnumerable<T>> FindByCondition(Expression<Func<T, object>> cretiria, Expression<Func<T, object>> expression);
    Task<T> FindById(Expression<Func<T, bool>> expression);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}
