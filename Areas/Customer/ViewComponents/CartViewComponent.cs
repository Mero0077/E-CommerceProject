using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceFIdentityScaff.Areas.Customer.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;

        public CartViewComponent(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var appUserId = userManager.GetUserId(HttpContext.User);
            if (appUserId != null)
            {
                var cartTotal= unitOfWork.Cart.GetAll(c=>c.ApplicationUserId==appUserId).Count();
                return View(cartTotal);
            }
            return View(0);
        }
    }
}
