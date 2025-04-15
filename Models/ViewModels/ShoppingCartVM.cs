namespace E_CommerceFIdentityScaff.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> shoppingCarts { get; set; }
        public double OrderTotal { get; set; }
    }
}
