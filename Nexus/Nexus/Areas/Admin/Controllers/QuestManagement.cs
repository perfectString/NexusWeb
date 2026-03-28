using Microsoft.AspNetCore.Mvc;

namespace Nexus.Areas.Admin.Controllers
{
    public class QuestManagement : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
