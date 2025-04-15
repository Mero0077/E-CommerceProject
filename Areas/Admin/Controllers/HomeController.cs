using Microsoft.AspNetCore.Mvc;

namespace E_CommerceFIdentityScaff.Areas.Admin.Controllers
{
    [Area("Admin,SuperAdmin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
