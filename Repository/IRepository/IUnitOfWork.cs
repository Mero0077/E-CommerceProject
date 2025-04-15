namespace E_CommerceFIdentityScaff.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICartRepository Cart { get; }
        IUserRepository User { get; }

        public Task Commit();
    }
}
