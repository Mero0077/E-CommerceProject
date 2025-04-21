using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Repository;
using E_CommerceFIdentityScaff.Repository.IRepository;

namespace E_CommerceFIdentityScaff.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private ApplicationDbContext _context;
        public ICategoryRepository Category { get; set; }
        public IProductRepository Product { get; set; }
        public ICartRepository Cart { get; set; }

        public IUserRepository ApplicationUser { get; set; }

        public ICompanyRepository Company { get; set; }

        //public IOrderDetailRepository OrderDetail { get; set; }
        //public IOrderHeaderRepository OrderHeader { get; set; }
        public IOrderRepository Order { get; set; }
        public IOrderItemRepository OrderItem { get; set; }

        public UnitOfWork(ApplicationDbContext dbContext) 
        {
            _context = dbContext;
            Category= new CategoryRepository(dbContext);
            Product= new ProductRepository(dbContext);
            Cart= new CartRepository(dbContext);
            ApplicationUser = new UserRepository(dbContext);
            Company= new CompanyRepository(dbContext);
            //OrderDetail= new OrderDetailRepository(dbContext);
            //OrderHeader= new OrderHeaderRepository(dbContext);
            Order= new OrderRepository(dbContext);
            OrderItem= new OrderItemRepository(dbContext);

        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
