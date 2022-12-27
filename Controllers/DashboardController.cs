using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rbauthor.Models.DTO;

namespace rbauthor.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Display()
        {
            return View();
        }
    }
}
