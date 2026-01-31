using Microsoft.AspNetCore.Mvc;

namespace Nexus.Controllers
{
    public class FriendRequestsController : Controller
    {

        public IActionResult Index()
        {
            return this.Ok("I work");
        }
    }
}
