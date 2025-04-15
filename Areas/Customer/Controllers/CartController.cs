using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Models.ViewModels;
using E_CommerceFIdentityScaff.Repository;
using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceFIdentityScaff.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartVM shoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var appuserid = _userManager.GetUserId(User);

            shoppingCartVM = new()
            {
                shoppingCarts = _unitOfWork.Cart.GetAll(c => c.ApplicationUserId == appuserid, includes: [c => c.Product, c => c.ApplicationUser])
            };

            foreach (var item in shoppingCartVM.shoppingCarts)
            {
                shoppingCartVM.OrderTotal += item.Product.Price * item.count;
            }

            return View(shoppingCartVM);
        }

        public async Task<IActionResult> AddToCart(int ProductId, int count)
        {
            string? appUserId = _userManager.GetUserId(User);
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductId = ProductId,
                ApplicationUserId = appUserId,
                count = count
            };

            ShoppingCart cartInDb = await _unitOfWork.Cart.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == ProductId);

            if (cartInDb != null)
                cartInDb.count += count;
            else
                await _unitOfWork.Cart.Add(shoppingCart);

            await _unitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DecreaseCart(ShoppingCart ShoppingCart)
        {
            ShoppingCart cart= await _unitOfWork.Cart.GetOne(c=>c.Id == ShoppingCart.Id);
            if (cart != null)
            {
                if(cart.count<=1) _unitOfWork.Cart.Delete(cart);
                else cart.count = ShoppingCart.count;
            }
            await _unitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> IncreaseCart(ShoppingCart ShoppingCart)
        {
            ShoppingCart cart = await _unitOfWork.Cart.GetOne(c => c.Id == ShoppingCart.Id);
            if (cart != null)
            {
                cart.count = ShoppingCart.count;
            }
            await _unitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public async Task< IActionResult> DeleteCart(int id)
        {
            ShoppingCart cart = await _unitOfWork.Cart.GetOne(c => c.Id == id);
            if (cart != null) _unitOfWork.Cart.Delete(cart);
            await _unitOfWork.Commit();
            return RedirectToAction("Index");

        }
    }
}
