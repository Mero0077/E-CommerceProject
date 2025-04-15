using System.Linq.Expressions;

namespace E_CommerceFIdentityScaff.Repository.IRepository
{
    public interface IRepository<T> 
    {
        public Task Add(T entity);
        public Task AddAll(List<T> entities);
        public void Edit(T entity);
        public void Delete(T entity);
        public void DeleteAll(List<T> entities);
        public Task Commit();

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public Task<T>? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public Task< bool> Exists(Expression<Func<T, bool>> filter);
    }
}
