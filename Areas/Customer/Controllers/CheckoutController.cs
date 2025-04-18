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

        public IActionResult Summary()
        {
            var UserId = _userManager.GetUserId(User);
            var carts= _unitOfWork.Cart.GetAll(c=>c.ApplicationUserId == UserId, includes: [e=>e.Product]).ToList();
            ShoppingCartVM shoppingCartVM = new()
            {
                shoppingCarts = carts,
                OrderTotal= carts.Sum(c=>c.Product.Price* c.count),
                order=new()
            };
            return View(shoppingCartVM);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
        public async Task<IActionResult> Pay()
        {
            var userId = _userManager.GetUserId(User);
            var carts = _unitOfWork.Cart.GetAll(e => e.ApplicationUserId == userId, [e => e.Product]).ToList();

            // create order
            var order = await CreateOrder(userId, carts);

            // add stripe options 
            var options = CreateStripeOptions(order);

            // add lines
            AddStripeLines(carts, options);

            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);
            order.SessionId = session.Id;

            await _unitOfWork.Order.Commit();
            return Redirect(session.Url);
        }

        // create order action
        public async Task<Order> CreateOrder(string userId, List<ShoppingCart> carts)
        {
            Order order = new();
            order.ApplicationUserId = userId;
            order.OrderDate = DateTime.Now;
            order.OrderTotal = (double)carts.Sum(e => e.Product.Price * e.count);
            order.PaymentStatus = false;
            order.Status = OrderStatus.Pending;

            await _unitOfWork.Order.Add(order);
            await _unitOfWork.Commit();

            return order;
        }

        // create stripe options
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
                        UnitAmount = (long)cart.Product.Price * 100,
                    },
                    Quantity = cart.count,
                });
            }
        }
    }
}

