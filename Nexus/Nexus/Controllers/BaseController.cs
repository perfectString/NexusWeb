using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nexus.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected Guid GetUserId()
        {
            string? value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(value!);
        }
    }
}
