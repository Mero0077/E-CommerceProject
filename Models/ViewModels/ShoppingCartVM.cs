using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_CommerceFIdentityScaff.Models.ViewModels
{
    public class ShoppingCartVM
    {
        [ValidateNever]
        public IEnumerable<ShoppingCart> shoppingCarts { get; set; }
        public Order order { get; set; }
        public double OrderTotal { get; set; }
    }
}
