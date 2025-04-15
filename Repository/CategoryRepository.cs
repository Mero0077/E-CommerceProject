using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;

namespace E_CommerceFIdentityScaff.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository 
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
