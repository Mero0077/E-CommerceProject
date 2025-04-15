
using E_CommerceFIdentityScaff.Repository.IRepository;
using E_CommerceFIdentityScaff.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_CommerceFIdentityScaff.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var cats = _unitOfWork.Category.GetAll();
            return View(cats.ToList());
        }

        [HttpGet]
        public IActionResult Edit(int CategoryId)
        {
            var category = _unitOfWork.Category.GetOne(c => c.Id == CategoryId);
            return category != null ? View(category) : NotFound();

        }
        [HttpPost]
        public IActionResult Edit(Category category)
            {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Edit(category);
                _unitOfWork.Commit();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            bool DisplayOrdersExists = _unitOfWork.Category.GetAll().Any(c => c.DisplayOrder == category.DisplayOrder);
            if (DisplayOrdersExists)
            {
                ModelState.AddModelError("DisplayOrder", "Already Exists!");
                return View(category);
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Commit();
                TempData["success"] = "Category Created successfully";
                Response.Cookies.Append("notif", "dddd");/////////////////

                return RedirectToAction("Index");
            }
            return View();

        }
        public IActionResult Delete(Category category)
        {
            if (category != null)
            {
                _unitOfWork.Category.Delete(category);
                _unitOfWork.Commit();
                TempData["success"] = "Category Deleted successfully";

            }

            return category != null ? RedirectToAction("Index") : View();

        }
    }
}
