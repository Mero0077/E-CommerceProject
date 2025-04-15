using Microsoft.AspNetCore.Mvc;

namespace E_CommerceFIdentityScaff.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
