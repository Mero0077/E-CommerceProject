using System.Linq.Expressions;

namespace E_CommerceFIdentityScaff.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Add(T entity);
        Task AddAll(List<T> entities);
        void Edit(T entity);
        void Delete(T entity);
        void DeleteAll(List<T> entities);
        Task Commit();
        IQueryable<T> GetAll(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true);

        Task<T> GetOne(
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>[]? includes = null,
            bool tracked = true);

        Task<bool> Exists(Expression<Func<T, bool>> filter);
    }

}
