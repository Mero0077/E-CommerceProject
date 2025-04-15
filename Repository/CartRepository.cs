using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;

namespace E_CommerceFIdentityScaff.Repository
{
    public class CartRepository: Repository<ShoppingCart>,ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext applicationDbContext) :base(applicationDbContext) 
        {
            _context = applicationDbContext;
            
        }
    }
}
