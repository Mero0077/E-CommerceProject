using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Models.ViewModels;
using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_CommerceFIdentityScaff.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var UserId = _userManager.GetUserId(User);
            var carts = _unitOfWork.Cart.GetAll(c => c.ApplicationUserId == UserId, includes: [e => e.Product]).ToList();
            ShoppingCartVM shoppingCartVM = new()
            {
                shoppingCarts = carts,
                OrderTotal = carts.Sum(c => c.Product.Price * c.count),
                order = new()
            };
            return View(shoppingCartVM);
        }

        [HttpPost]
        public async Task<IActionResult> Summary(ShoppingCartVM shoppingCartVM)
        {

            var userId = _userManager.GetUserId(User);

            var carts = _unitOfWork.Cart.GetAll(e => e.ApplicationUserId == userId, [e => e.Product]).ToList();

            if (!ModelState.IsValid)
            {
                shoppingCartVM.shoppingCarts = carts;
                shoppingCartVM.OrderTotal = carts.Sum(e => e.Product.Price * e.count);

                return View(shoppingCartVM);
            }

            var order = new Order();
            var user = await _userManager.GetUserAsync(User);
            order.Name = shoppingCartVM.order.Name;
            order.StreetAddress = shoppingCartVM.order.StreetAddress;
            order.City = shoppingCartVM.order.City;
            order.State = shoppingCartVM.order.State;
            order.PhoneNumber = shoppingCartVM.order.PhoneNumber;
            order.PostalCode = shoppingCartVM.order.PostalCode;
            order.ApplicationUserId = user.Id;
            order.OrderDate = DateTime.Now;
            order.OrderTotal = (double)carts.Sum(e => e.Product.Price * e.count);
            order.PaymentStatus = false;
            order.Status = OrderStatus.Pending;

            var options = CreateStripeOptions(order);

            // add lines  
            AddStripeLines(carts, options);

            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);
            order.SessionId = session.Id;

            await _unitOfWork.Order.Add(order);
            await _unitOfWork.Order.Commit();
            return Redirect(session.Url);
        }
    
        

        public async Task<IActionResult> SuccessAsync()
        {
            var userId = _userManager.GetUserId(User);
            var userCarts = _unitOfWork.Cart.GetAll(c => c.ApplicationUserId == userId).ToList();
            _unitOfWork.Cart.DeleteAll(userCarts);
            await _unitOfWork.Commit();

            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
   
        public Stripe.Checkout.SessionCreateOptions CreateStripeOptions(Order order)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.Id}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
            };
            return options;
        }

        // add stripe lines
        public void AddStripeLines(List<ShoppingCart> carts, Stripe.Checkout.SessionCreateOptions options)
        {
            foreach (var cart in carts)
            {
                if (cart.count < 1 || cart.Product.Price <= 0) continue;

                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = cart.Product.Title,
                            Description = cart.Product.Description,
                        },
                        UnitAmount = (long)(cart.Product.Price * 100), // FIXED LINE
                    },
                    Quantity = cart.count,
                });
            }
        }

    }
}

