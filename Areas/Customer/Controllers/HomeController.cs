using System.Diagnostics;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;
using E_CommerceFIdentityScaff.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace E_CommerceFIdentityScaff.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var prods = _unitOfWork.Product.GetAll().ToList();
            return View(prods);
        }

        [HttpGet]
       
        public async Task<IActionResult> Details(int Id)
        {
            ShoppingCart shoppingCart = new()
            {
             Product = await _unitOfWork.Product.GetOne(p => p.Id == Id, includes: [p => p.Category]),
             count=1,
             ProductId = Id

            };

            return View(shoppingCart);
        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        //{
        //    var claims = (ClaimsIdentity)User.Identity;
        //    var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

        //   shoppingCart.ApplicationUserId = userId;
        //    shoppingCart.Id = 0;

        //    ShoppingCart cartfromDb = await _unitOfWork.Cart.GetOne(e=>e.ApplicationUserId == userId && e.ProductId==shoppingCart.ProductId,null,false);
        //    if (cartfromDb != null)
        //    {
        //        cartfromDb.count += shoppingCart.count;
        //        _unitOfWork.Cart.Edit(cartfromDb);

        //    }
        //    else
        //        await _unitOfWork.Cart.Add(shoppingCart);


        //    await _unitOfWork.Commit();

        //    return RedirectToAction("Index");
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
