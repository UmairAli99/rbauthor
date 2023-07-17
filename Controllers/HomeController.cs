using Microsoft.AspNetCore.Mvc;
using rbauthor.Models;
using System.Diagnostics;
using System.Security.Policy;

namespace rbauthor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Contacts contacts)
        {
            FirebaseContext context = new FirebaseContext();

            var sitesRef = context.Client.Child("Contacts");
            sitesRef.PostAsync(new Contacts
            {
                name = contacts.name,
                email = contacts.email,
                subject = contacts.subject,
                message = contacts.message
            });
            return RedirectToAction("Index", "Home");
        }
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contacts contacts)
        {
            FirebaseContext context = new FirebaseContext();

            var sitesRef = context.Client.Child("Contacts");
            sitesRef.PostAsync(new Contacts
            {
                name = contacts.name,
                email = contacts.email,
                subject = contacts.subject,
                message = contacts.message
            });
            return View();
        }
        public IActionResult Armed()
        {
            return View();
        }
        public IActionResult Unarmed()
        {
            return View();
        }
        public IActionResult Airport()
        {
            return View();
        }
        public IActionResult Commercial()
        {
            return View();
        }
        public IActionResult Executive()
        {
            return View();
        }
        public IActionResult Event()
        {
            return View();
        }
        public IActionResult FAQs()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}