using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Models;

namespace Nexus.Controllers
{
    public class UserController : Controller
    {
        /*  every time someone access this the app will call a ctor to access the dbcontext 
         and save it in the readonly
         ctor injection == makes the code more readable without rewriting the same code  */

        private readonly NexusDbContext dbContext;   
        public UserController(NexusDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allUsers = dbContext
                .Users
                .Include(u => u.UserInterest)
                .ThenInclude(u=> u.Interest)
                .AsSplitQuery()
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Age,
                    u.City,
                    u.DesiredConnection,
                    u.Bio,
                    Interests = u.UserInterest
                    .Select(i => new
                    {
                        i.Interest.Name
                    })
                    .ToArray()
                })
                .OrderBy(u => u.Age)
                .ThenBy(u => u.Name)
                .ThenBy(u => u.City)
                .ToArray();

            return View(allUsers);
        }
    }
}
