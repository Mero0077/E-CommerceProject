using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceFIdentityScaff.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,SuperAdmin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string? query,int page=1)
        {
            var users= _unitOfWork.User.GetAll();

            if(query!=null) 
                users= users.Where(u => u.Id.Contains(query) || u.UserName.Contains(query) || u.Email.Contains(query) || u.Address.Contains(query));

            users = users.Skip((page-1)*2).Take(2);
           double PageNums= Math.Ceiling(users.Count() / (double)2);

            if(page>PageNums+1) return RedirectToAction("NotFound");
            ViewBag.PageNums = PageNums;

            return View(users.ToList());
        }
    }
}
