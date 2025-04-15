using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;

namespace E_CommerceFIdentityScaff.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
