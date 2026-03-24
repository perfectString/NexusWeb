using Microsoft.AspNetCore.Mvc;

namespace Nexus.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
