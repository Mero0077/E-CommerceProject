using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Models.ViewModels;
using E_CommerceFIdentityScaff.Repository.IRepository;
using E_CommerceFIdentityScaff.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace E_CommerceFIdentityScaff.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
        public class UserController : Controller
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ApplicationDbContext applicationDbContext; 
            private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            this.applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public IActionResult Index(string? query,int page=1,int PageSize=5)
        {
            var users = _unitOfWork.ApplicationUser.GetAll().ToList(); 
            var userRoles = applicationDbContext.UserRoles.ToList();
            var roles = applicationDbContext.Roles.ToList();

            if (!string.IsNullOrEmpty(query))
            {
                users = users.Where(u =>
                    u.Id.Contains(query) ||
                    u.UserName.Contains(query) ||
                    u.Email.Contains(query) ||
                    u.Address.Contains(query)
                ).ToList();
            }

            var userWithRoles = users.Select(user =>
            {
                var userRole = userRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                var roleName = "No Role";

                if (userRole != null)
                {
                    var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                    if (role != null)
                        roleName = role.Name;
                }

                return new UserWithRoleVM
                {
                    User = user,
                    Role = roleName,
                    RoleList = []
                };
            }).ToList();

            PaginatedVM<UserWithRoleVM> vm= PaginationHelper.Paginate(userWithRoles, page, PageSize);

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(userVM.ApplicationUser, userVM.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
                return View(userVM);

         }


        public async Task<IActionResult> Edit(string Id)
        {
            // Use Identity's user manager to get a fully tracked ApplicationUser
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null) return NotFound();

            var roles = applicationDbContext.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            });

            var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "";

            UserWithRoleVM vm = new()
            {
                User = user,
                Role = userRole,
                RoleList = roles
            };

            return View(vm);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(UserWithRoleVM userWithRole)
        {
            if (!ModelState.IsValid)
                return View(userWithRole);

            var userInDb = await _userManager.FindByIdAsync(userWithRole.User.Id);

            if (userInDb == null)
                return NotFound();

          
            userInDb.UserName = userWithRole.User.UserName;
            userInDb.Email = userWithRole.User.Email;
            userInDb.PhoneNumber = userWithRole.User.PhoneNumber;
            userInDb.Address = userWithRole.User.Address;

            // Update the user in the DB
            var updateResult = await _userManager.UpdateAsync(userInDb);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userWithRole);
            }

            // Remove current roles
            var currentRoles = await _userManager.GetRolesAsync(userInDb);
            await _userManager.RemoveFromRolesAsync(userInDb, currentRoles);

            // Add the selected role
            if (!string.IsNullOrWhiteSpace(userWithRole.Role))
            {
                await _userManager.AddToRoleAsync(userInDb, userWithRole.Role);
            }
           await _unitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _unitOfWork.ApplicationUser.GetOne(u => u.Id == id, tracked: false);
            if (user != null)
            {
                _unitOfWork.ApplicationUser.Delete(user);
                await _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> LockUnlockAsync(string id)
        {
            var user = await _unitOfWork.ApplicationUser.GetOne(e => e.Id == id);

            if (user == null)
                return NotFound();

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
            {
                // User is locked — unlock
                user.LockoutEnd = DateTime.UtcNow;
            }
            else
            {
                // User is unlocked — lock for 100 years!!!!!!!!!!!!!!!!
                user.LockoutEnd = DateTime.UtcNow.AddYears(100);
            }

            _unitOfWork.ApplicationUser.Edit(user);
            await _unitOfWork.Commit();

            return RedirectToAction(nameof(Index));
        }



        //public IActionResult Index(string? query, int page = 1)
        //{
        //    var users = _unitOfWork.User.GetAll();

        //    if (query != null)
        //        users = users.Where(u => u.Id.Contains(query) || u.UserName.Contains(query) || u.Email.Contains(query) || u.Address.Contains(query));

        //    // users = users.Skip((page-1)*2).Take(2);
        //    //double PageNums= Math.Ceiling(users.Count() / (double)2);

        //    // if(page>PageNums+1) return RedirectToAction("NotFound");
        //    // ViewBag.PageNums = PageNums;

        //    return View(users.ToList());
        //}

        //[HttpPost]
        //public async Task<IActionResult> ToggleBlock(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        if (user.LockoutEnabled) // If the user is blocked
        //        {
        //            // Unblock user (reset the lockout end date)
        //            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
        //            user.LockoutEnabled = false;  // Mark as not locked out
        //        }
        //        else
        //        {
        //            // Block user (set a permanent lockout end date)
        //            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        //            user.LockoutEnabled = true;  // Mark as locked out
        //        }

        //        // Save changes to the user
        //        await _userManager.UpdateAsync(user);
        //    }

        //    return RedirectToAction("Index"); // Or wherever you want to redirect after toggling
        //}

        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var user = _unitOfWork.User.GetOne(c => c.Id == id.ToString());
        //    return user != null ? View(user) : View();
        //}
        //[HttpPost]
        //public IActionResult Edit(       company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Edit(company);
        //        _unitOfWork.Commit();
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Create(Company company)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Add(company);
        //        _unitOfWork.Commit();
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var company = await _unitOfWork.Company.GetOne(c => c.Id == id);
        //    if (company != null)
        //    {
        //        _unitOfWork.Company.Delete(company);
        //        await _unitOfWork.Commit();
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
    }

}

