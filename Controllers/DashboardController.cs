using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rbauthor.Models;
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
        [HttpGet]
        public IActionResult SaveFeedback()
        {
            return View();
        }
        [Authorize(Roles = "user")]
        [HttpPost]
        public IActionResult SaveFeedback(Feedback feedback)
        {
            FirebaseContext context = new FirebaseContext();
            var feedbackRef = context.Client.Child("feedbacks");
            feedbackRef.PostAsync(new Feedback
            {
                GuardId = feedback.GuardId,
                GuardName= feedback.GuardName,
                ClientName= feedback.ClientName,
                Date= feedback.Date,
                SiteAddress = feedback.SiteAddress,
                Stars = feedback.Stars,
                Comment = feedback.Comment


            });

            return RedirectToAction("SaveFeedback", "Dashboard");
        }
    }
}
