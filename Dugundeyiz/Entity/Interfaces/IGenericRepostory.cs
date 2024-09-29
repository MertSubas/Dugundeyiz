using System.Linq.Expressions;

namespace Dugundeyiz.Entity.Interfaces
{
    public interface IGenericRepostory<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, params Expression<Func<T, object>>[] includes);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
