namespace E_CommerceFIdentityScaff.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICartRepository Cart { get; }
        IUserRepository ApplicationUser { get; }
        ICompanyRepository Company { get; }
        //IOrderHeaderRepository OrderHeader { get; }
        //IOrderDetailRepository OrderDetail { get; }
        IOrderRepository Order { get; }
        IOrderItemRepository OrderItem { get; }

        public Task Commit();
    }
}
