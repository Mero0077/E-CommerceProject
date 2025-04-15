using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace E_CommerceFIdentityScaff.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbSet<T> dbSet;
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }
        public async Task Add(T entity)
        {
          await  dbSet.AddAsync(entity);
        }
        public async Task AddAll(List<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void DeleteAll(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public async Task<T?> GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return await GetAll(filter, includes, tracked).FirstOrDefaultAsync();
        }

        public async Task< bool> Exists(Expression<Func<T, bool>> filter)
        {
            return await dbSet.AnyAsync(filter);
        }

    }

}