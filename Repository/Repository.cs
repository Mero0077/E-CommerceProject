using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace E_CommerceFIdentityScaff.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        public DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public async Task Add(T entity) => await dbSet.AddAsync(entity);
        public async Task AddAll(List<T> entities) => await dbSet.AddRangeAsync(entities);
        public void Edit(T entity) => dbSet.Update(entity);
        public void Delete(T entity) => dbSet.Remove(entity);
        public void DeleteAll(List<T> entities) => dbSet.RemoveRange(entities);
        public async Task Commit() => await _context.SaveChangesAsync();

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (!tracked) query = query.AsNoTracking();
            if (filter != null) query = query.Where(filter);
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
            return query;
        }

        public async Task<T> GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return await GetAll(filter, includes, tracked).FirstOrDefaultAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> filter)
        {
            return await dbSet.AnyAsync(filter);
        }
    }

}