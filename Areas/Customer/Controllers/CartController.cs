using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Models.ViewModels;
using E_CommerceFIdentityScaff.Repository;
using E_CommerceFIdentityScaff.Repository.IRepository;
using E_CommerceFIdentityScaff.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace E_CommerceFIdentityScaff.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var UserId = _userManager.GetUserId(User);
            var carts= _unitOfWork.Cart.GetAll(c=>c.ApplicationUserId == UserId, includes:[e=>e.Product]);

            return View(carts.ToList());
        }
        public async  Task<IActionResult> AddToCart(int ProductId, int count)
        {
            var UserId= _userManager.GetUserId(User);
            var CartInDb= await _unitOfWork.Cart.GetOne(e=>e.ApplicationUserId == UserId && e.ProductId==ProductId);


            if (CartInDb != null)
            
                CartInDb.count += count;
            else
            {
                var cart = new ShoppingCart()
                {
                    ApplicationUserId = UserId,
                    count = count,
                    ProductId = ProductId
                };
                await _unitOfWork.Cart.Add(cart);
            }
            await _unitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increment(int Id)
        {
            var UserId = _userManager.GetUserId(User);
            var cart = await _unitOfWork.Cart.GetOne(e => e.ProductId == Id && e.ApplicationUserId == UserId);

            cart.count++;
            _unitOfWork.Cart.Edit(cart);
            await _unitOfWork.Commit();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Decrement(int Id)
        {
            var UserId = _userManager.GetUserId(User);
            var cart = await _unitOfWork.Cart.GetOne(e => e.ProductId == Id && e.ApplicationUserId == UserId);

            cart.count--;
            _unitOfWork.Cart.Edit(cart);
            await _unitOfWork.Commit();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int Id)
        {
            var userApp = _userManager.GetUserId(User);
            var cart = await _unitOfWork.Cart.GetOne(e => e.ProductId == Id && e.ApplicationUserId == userApp);
            if (cart != null)
            {
                _unitOfWork.Cart.Delete(cart);
                await _unitOfWork.Commit();
            }
            return RedirectToAction("Index");
        }
       
    //public IActionResult Index()
    //{
    //    var appuserid = _userManager.GetUserId(User);

    //    shoppingCartVM = new()
    //    {
    //        shoppingCarts = _unitOfWork.Cart.GetAll(c => c.ApplicationUserId == appuserid, includes: [c => c.Product, c => c.ApplicationUser]),
    //        orderHeader=new()
    //    };

    //    foreach (var item in shoppingCartVM.shoppingCarts)
    //    {
    //        shoppingCartVM.orderHeader.OrderTotal += item.Product.Price * item.count;
    //    }

    //    return View(shoppingCartVM);
    //}

    //public async Task<IActionResult> AddToCart(int ProductId, int count)
    //{
    //    string? appUserId = _userManager.GetUserId(User);
    //    ShoppingCart shoppingCart = new ShoppingCart()
    //    {
    //        ProductId = ProductId,
    //        ApplicationUserId = appUserId,
    //        count = count
    //    };

    //    ShoppingCart cartInDb = await _unitOfWork.Cart.GetOne(e => e.ApplicationUserId == appUserId && e.ProductId == ProductId);

    //    if (cartInDb != null)
    //        cartInDb.count += count;
    //    else
    //        await _unitOfWork.Cart.Add(shoppingCart);

    //    await _unitOfWork.Commit();

    //    return RedirectToAction("Index");
    //}

    //public async Task<IActionResult> DecreaseCart(ShoppingCart ShoppingCart)
    //{
    //    ShoppingCart cart= await _unitOfWork.Cart.GetOne(c=>c.Id == ShoppingCart.Id);
    //    if (cart != null)
    //    {
    //        if(cart.count<=1) _unitOfWork.Cart.Delete(cart);
    //        else cart.count = ShoppingCart.count;
    //    }
    //    await _unitOfWork.Commit();

    //    return RedirectToAction("Index");
    //}

    //public async Task<IActionResult> IncreaseCart(ShoppingCart ShoppingCart)
    //{
    //    ShoppingCart cart = await _unitOfWork.Cart.GetOne(c => c.Id == ShoppingCart.Id);
    //    if (cart != null)
    //    {
    //        cart.count = ShoppingCart.count;
    //    }
    //    await _unitOfWork.Commit();

    //    return RedirectToAction("Index");
    //}

    //public async Task< IActionResult> DeleteCart(int id)
    //{
    //    ShoppingCart cart = await _unitOfWork.Cart.GetOne(c => c.Id == id);
    //    if (cart != null) _unitOfWork.Cart.Delete(cart);
    //    await _unitOfWork.Commit();
    //    return RedirectToAction("Index");

    //}

    //public async Task<IActionResult> Summary()
    //{
    //    var appuserid = _userManager.GetUserId(User);
    //    var appUser = await _userManager.FindByIdAsync(appuserid);

    //    shoppingCartVM = new()
    //    {
    //        shoppingCarts = _unitOfWork.Cart.GetAll(c => c.ApplicationUserId == appuserid, includes: [c => c.Product, c => c.ApplicationUser]),
    //        orderHeader = new()
    //    };

    //    shoppingCartVM.orderHeader.Name = appUser.UserName;
    //    shoppingCartVM.orderHeader.PhoneNumber = appUser.PhoneNumber;
    //    shoppingCartVM.orderHeader.City = appUser.City;
    //    shoppingCartVM.orderHeader.PostalCode = appUser.PostalCode;
    //    shoppingCartVM.orderHeader.State = appUser.State;
    //    shoppingCartVM.orderHeader.StreetAddress = appUser.Address;

    //    foreach (var item in shoppingCartVM.shoppingCarts)
    //    {
    //        shoppingCartVM.orderHeader.OrderTotal += item.Product.Price * item.count;
    //    }

    //    return View(shoppingCartVM);
    //}
    //[HttpPost]
    //[ActionName("Summary")]
    //public async Task <IActionResult> SummaryPost()
    //{
    //    var appuserid = _userManager.GetUserId(User);
    //    var appuser= await _userManager.GetUserAsync(User);

    //    shoppingCartVM.shoppingCarts = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == appuserid, includes: [u => u.Product]);
    //    shoppingCartVM.orderHeader.OrderDate = DateTime.Now;
    //    shoppingCartVM.orderHeader.ApplicationUserId = appuserid;
    //    shoppingCartVM.orderHeader.ApplicationUser = appuser;

    //    foreach (var item in shoppingCartVM.shoppingCarts)
    //    {
    //        shoppingCartVM.orderHeader.OrderTotal += item.Product.Price * item.count;
    //    }

    //    if(shoppingCartVM.orderHeader.ApplicationUser.CompanyId.GetValueOrDefault()==0)
    //    {
    //        shoppingCartVM.orderHeader.OrderStatus=PaymentAndShipping.StatusPending;
    //        shoppingCartVM.orderHeader.PaymentStatus = PaymentAndShipping.PaymentStatusPending;
    //    }
    //    else
    //    {
    //        // Company Account
    //        shoppingCartVM.orderHeader.OrderStatus = PaymentAndShipping.StatusApproved;
    //        shoppingCartVM.orderHeader.PaymentStatus = PaymentAndShipping.PaymentStatusDelayedPayment;
    //    }
    //    await _unitOfWork.OrderHeader.Add(shoppingCartVM.orderHeader);
    //    await _unitOfWork.Commit();

    //    foreach(var cart in shoppingCartVM.shoppingCarts)
    //    {
    //        OrderDetail detail = new OrderDetail()
    //        {
    //            ProductId = cart.ProductId,
    //            OrderHeaderId = shoppingCartVM.orderHeader.Id,
    //            Price = cart.Product.Price,
    //            Count = cart.count,
    //        };
    //        await _unitOfWork.OrderDetail.Add(detail);
    //        await _unitOfWork.Commit();
    //    }
    //    return RedirectToAction("OrderConfirmation", new {id=shoppingCartVM.orderHeader.Id});
    //}

    //public IActionResult OrderConfirmation(int Id)
    //{
    //    return View(Id);
    //}
    //public async Task<IActionResult> Pay()
    //{
    //    var appuserid = _userManager.GetUserId(User);

    //    double sum = 0;
    //    var shoppingCarts = _unitOfWork.Cart.GetAll(c => c.ApplicationUserId == appuserid, includes: [c => c.Product]);
    //    foreach (var item in shoppingCarts)
    //    {
    //        sum += item.Product.Price * item.count;
    //    }


    //    var options = new SessionCreateOptions
    //    {
    //        PaymentMethodTypes = new List<string> { "card" },
    //        LineItems = new List<SessionLineItemOptions>(),

    //    //   
    //        Mode = "payment",
    //        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/success",
    //        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/checkout/cancel",
    //    };

    //    foreach(var item in shoppingCarts)
    //    {
    //        options.LineItems.Add(new SessionLineItemOptions
    //        {
    //            PriceData = new SessionLineItemPriceDataOptions
    //            {
    //                Currency = "usd",
    //                ProductData = new SessionLineItemPriceDataProductDataOptions
    //                {
    //                    Name = item.Product.Title,
    //                    Description = item.Product.Description,
    //                },
    //                UnitAmount = (long)item.Product.Price*100,
    //            },
    //            Quantity = item.count,
    //        });


    //}

    //    var service = new SessionService();
    //    var session = service.Create(options);

    //    await _unitOfWork.Order.Add(new()
    //    {
    //        ApplicationUserId = appuserid,
    //        OrderDate = DateTime.Now,
    //        OrderTotal = shoppingCarts.Sum(e => e.Product.Price * e.count),
    //        PaymentStatus = false,
    //        Status = OrderStatus.InProgress,
    //        SessionId=session.Id
    //    });
    //    await _unitOfWork.Commit();

    //    return Redirect(session.Url);

    //}



}
}
