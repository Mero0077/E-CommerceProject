using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;


namespace E_CommerceFIdentityScaff.Repository
    {
    public class OrderRepository : Repository<Order>, IOrderRepository
        {
            private readonly ApplicationDbContext _context;
            public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
            {
                _context = dbContext;
            }

       
       }
    }
