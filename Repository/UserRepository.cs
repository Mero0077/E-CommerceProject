using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;

namespace E_CommerceFIdentityScaff.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            applicationDbContext = dbContext;
        }
    }
}
