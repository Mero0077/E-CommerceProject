    using E_CommerceFIdentityScaff.Data;
    using E_CommerceFIdentityScaff.Repository.IRepository;
    using Stripe.Climate;
using System.Linq.Expressions;

namespace E_CommerceFIdentityScaff.Repository
    {
        public class OrderRepository : Repository<Order>, IOrderRepository
        {
            private readonly ApplicationDbContext _context;
            public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
            {
                _context = dbContext;
            }

        public Task Add(Models.Order entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAll(List<Models.Order> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(Models.Order entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<Models.Order> entities)
        {
            throw new NotImplementedException();
        }

        public void Edit(Models.Order entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(Expression<Func<Models.Order, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Models.Order> GetAll(Expression<Func<Models.Order, bool>>? filter = null, Expression<Func<Models.Order, object>>[]? includes = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Order>? GetOne(Expression<Func<Models.Order, bool>>? filter = null, Expression<Func<Models.Order, object>>[]? includes = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }
    }
    }
