using E_CommerceFIdentityScaff.Models;
using E_CommerceFIdentityScaff.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_CommerceFIdentityScaff.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="SuperAdmin,Admin,Company")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public IActionResult Index(string? query,int page=1)
        {
            var companies= _unitOfWork.Company.GetAll();
            if (query != null)
                companies = companies.Where(u => u.Id.ToString().Contains(query) || u.Name.Contains(query) || u.Number.Contains(query));

            companies = companies.Skip((page - 1) * 2).Take(2);
            double PageNums = Math.Ceiling(companies.Count() / (double)2);

            if (page > PageNums + 1) return RedirectToAction("NotFound");

            ViewBag.PageNums = PageNums;
           
            return View(companies.ToList());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var company= _unitOfWork.Company.GetOne(c=>c.Id == id);
            return company!=null ? View(company) : View();
        }
        [HttpPost]
        public IActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Edit(company);
                _unitOfWork.Commit();
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
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Company.Add(company);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task< IActionResult> Delete(int id)
        {
            var company= await _unitOfWork.Company.GetOne(c=>c.Id==id);
            if (company!=null)
            {
                _unitOfWork.Company.Delete(company);
               await _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
