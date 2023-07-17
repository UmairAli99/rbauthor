using Firebase.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rbauthor.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Firebase.Database.Query;
using Google.Api;

namespace rbauthor.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Display()
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            // Count the number of sites
            var sites = await _firebaseClient.Child("sites").OnceAsync<Site>();
            int siteCount = sites.Count();
            List<Site> siteList = new List<Site>();
            foreach (var siteSnapshot in sites)
            {
                Site site = siteSnapshot.Object;
                siteList.Add(site);
            }

            var Guards = await _firebaseClient.Child("Guards").OnceAsync<Guard>();
            int guardsCount = Guards.Count();

            var shifts = await _firebaseClient.Child("shifts").OnceAsync<Shift>();
            int shiftsCount = shifts.Count();
            List<Shift> shiftList = new List<Shift>();

            var guards = await _firebaseClient.Child("Guards").OnceAsync<Guard>();
            List<Guard> guardList = guards.Select(g => g.Object).ToList();

            foreach (var shiftSnapshot in shifts)
            {
                Shift shift = shiftSnapshot.Object;

                // Retrieve the selected guards using the GuardIds collection
                List<Guard> selectedGuards = new List<Guard>();
                if (shift.GuardIds != null)
                {
                    foreach (int guardId in shift.GuardIds)
                    {
                        var guard = guardList.FirstOrDefault(g => g.guardId == guardId);
                        if (guard != null)
                        {
                            selectedGuards.Add(guard);
                        }
                    }
                }

                shift.Guards = selectedGuards;
                shift.GuardNames = selectedGuards.Select(g => g.Name).ToList();

                shiftList.Add(shift);
            }
            DisplayViewModel viewModel = new DisplayViewModel
            {
                Shifts = shiftList,
                Sites = siteList
            };
           
            ViewBag.SiteCount = siteCount;
            ViewBag.GuardCount = guardsCount;
            ViewBag.ShiftCount = shiftsCount;

            return View(viewModel);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult SaveSites()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult SaveSites(Site site)
        {
            FirebaseContext context = new FirebaseContext();

            var sitesRef = context.Client.Child("sites");
            sitesRef.PostAsync(new Site
            {
                SiteId = site.SiteId,
                SiteAddress = site.SiteAddress,
                Latitude = site.Latitude,
                Longitude = site.Longitude
            });

            return RedirectToAction("Display");
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetSites()
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            var sites = await _firebaseClient.Child("sites").OnceAsync<Site>();
            List<Site> siteList = new List<Site>();
            foreach (var siteSnapshot in sites)
            {
                Site site = siteSnapshot.Object;
                siteList.Add(site);
            }
            return View(siteList);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllSites()                                             
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            var sites = await _firebaseClient.Child("sites").OnceAsync<Site>();
            List<Site> siteList = new List<Site>();
            foreach (var siteSnapshot in sites)
            {
                Site site = siteSnapshot.Object;
                siteList.Add(site);
            }
            return View(siteList);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
public IActionResult DeleteSite(int siteId)
{
    FirebaseContext context = new FirebaseContext();
     var sites = context.Client.Child("sites");
            var siteQuery = sites.OrderBy("SiteId").EqualTo(siteId).OnceAsync<Site>();
            var site = siteQuery.Result.FirstOrDefault();
            if (site != null)
            {
                context.Client.Child("sites").Child(site.Key).DeleteAsync();
            }

            // Redirect back to the DisplayAllShifts action
            return RedirectToAction("GetAllSites", new { successMessage = true });

}
        [Authorize(Roles = "admin")]
        [HttpPost]
public IActionResult DeleteAllSites()
{
    FirebaseContext context = new FirebaseContext();
    var sites = context.Client.Child("sites");
    sites.DeleteAsync();

    // Redirect back to the GetSites action
    return RedirectToAction("GetSites", new { successMessage = true });
}
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetGuards()
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            var Guards = await _firebaseClient.Child("Guards").OnceAsync<Guard>();
            List<Guard> guardList = new List<Guard>();
            foreach (var guardSnapshot in Guards)
            {
                Guard guard = guardSnapshot.Object;
                guardList.Add(guard);
            }
            return View(guardList);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteGuard(int guardId)
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            // Query the guards to find the document with matching GuardId
            var guardQuery = await _firebaseClient.Child("Guards")
                .OrderBy("guardId")
                .EqualTo(guardId)
                .OnceAsync<Guard>();

            // Get the first matching guard document
            var guard = guardQuery.FirstOrDefault();

            // Delete the guard document if it exists
            if (guard != null)
            {
                await _firebaseClient.Child("Guards").Child(guard.Key).DeleteAsync();
            }

            // Redirect back to the GetGuards action
            return RedirectToAction("GetGuards", "Admin");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult DeleteAllGuards()
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            _firebaseClient.Child("Guards").DeleteAsync();

            // Redirect back to the GetGuards action
            return RedirectToAction("GetGuards", "Admin");
        }

        //done
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateShift()
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            var sites = await _firebaseClient.Child("sites").OnceAsync<Site>();
            List<Site> siteList = new List<Site>();
            foreach (var siteSnapshot in sites)
            {
                Site site = siteSnapshot.Object;
                siteList.Add(site);
            }

            var guards = await _firebaseClient.Child("Guards").OnceAsync<Guard>();
            List<SelectListItem> guardList = new List<SelectListItem>();
            foreach (var guardSnapshot in guards)
            {
                Guard guard = guardSnapshot.Object;
                guardList.Add(new SelectListItem
                {
                    Value = guard.guardId.ToString(),
                    Text = guard.Name
                });
            }

            ViewBag.Sites = new SelectList(siteList, "SiteAddress", "SiteAddress"); // Use SiteAddress for both value and text
            ViewBag.Guards = guardList;

            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateShift(Shift shift)
        {
            FirebaseContext context = new FirebaseContext();
            var shiftsRef = context.Client.Child("shifts");

            // Retrieve the selected guards using the GuardIds collection
            List<Guard> selectedGuards = new List<Guard>();
            if (shift.GuardIds != null)
            {
                foreach (int guardId in shift.GuardIds)
                {
                    Guard guard = await context.Client.Child("Guards").Child(guardId.ToString()).OnceSingleAsync<Guard>();
                    selectedGuards.Add(guard);
                }
            }

            // Set the Guards property of the shift model
            shift.Guards = selectedGuards;

            await shiftsRef.PostAsync(shift);

            return RedirectToAction("Display", "Admin");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DisplayShifts()
        {
            FirebaseContext context = new FirebaseContext();
            var shifts = await context.Client.Child("shifts").OrderBy("ShiftId").OnceAsync<Shift>();

            List<Shift> shiftList = new List<Shift>();

            var guards = await context.Client.Child("Guards").OnceAsync<Guard>();
            List<Guard> guardList = guards.Select(g => g.Object).ToList();

            foreach (var shiftSnapshot in shifts)
            {
                Shift shift = shiftSnapshot.Object;

                // Retrieve the selected guards using the GuardIds collection
                List<Guard> selectedGuards = new List<Guard>();
                if (shift.GuardIds != null)
                {
                    foreach (int guardId in shift.GuardIds)
                    {
                        var guard = guardList.FirstOrDefault(g => g.guardId == guardId);
                        if (guard != null)
                        {
                            selectedGuards.Add(guard);
                        }
                    }
                }

                shift.Guards = selectedGuards;
                shift.GuardNames = selectedGuards.Select(g => g.Name).ToList();

                shiftList.Add(shift);
            }
            return View(shiftList);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Contacts()
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            var contacts = await _firebaseClient.Child("Contacts").OnceAsync<Contacts>();
            List<Contacts> contactList = new List<Contacts>();
            foreach (var contactSnapshot in contacts)
            {
                Contacts contact = contactSnapshot.Object;
                contactList.Add(contact);
            }
            return View(contactList); 
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult DeleteContact(string email)
        {
            FirebaseContext context = new FirebaseContext();
            var contacts = context.Client.Child("Contacts");

            // Query the contacts to find the document with matching email
            var contactQuery = contacts.OrderBy("email").EqualTo(email).OnceAsync<Contacts>();

            // Get the first matching contact document
            var contact = contactQuery.Result.FirstOrDefault();

            // Delete the contact document if it exists
            if (contact != null)
            {
                context.Client.Child("Contacts").Child(contact.Key).DeleteAsync();
            }

            // Redirect back to the Contacts action
            return RedirectToAction("Contacts");
        }


        [Authorize(Roles = "admin")]

        [HttpGet]
        public async Task<IActionResult> TrackGuards()
        {
            FirebaseClient _firebaseClient = new FirebaseClient("https://tracking-system-9fc3f-default-rtdb.firebaseio.com/");

            var Guards = await _firebaseClient.Child("Guards").OnceAsync<Guard>();
            List<Guard> guardList = new List<Guard>();
            foreach (var guardSnapshot in Guards)
            {
                Guard guard = guardSnapshot.Object;
                guardList.Add(guard);
            }
            return View(guardList);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DisplayAllShifts()
        {
            FirebaseContext context = new FirebaseContext();
            var shifts = await context.Client.Child("shifts").OrderBy("ShiftId").OnceAsync<Shift>();

            List<Shift> shiftList = new List<Shift>();

            var guards = await context.Client.Child("Guards").OnceAsync<Guard>();
            List<Guard> guardList = guards.Select(g => g.Object).ToList();

            foreach (var shiftSnapshot in shifts)
            {
                Shift shift = shiftSnapshot.Object;

                // Retrieve the selected guards using the GuardIds collection
                List<Guard> selectedGuards = new List<Guard>();
                if (shift.GuardIds != null)
                {
                    foreach (int guardId in shift.GuardIds)
                    {
                        var guard = guardList.FirstOrDefault(g => g.guardId == guardId);
                        if (guard != null)
                        {
                            selectedGuards.Add(guard);
                        }
                    }
                }

                shift.Guards = selectedGuards;
                shift.GuardNames = selectedGuards.Select(g => g.Name).ToList();

                shiftList.Add(shift);
            }

            return View(shiftList);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult DeleteShift(int shiftId)
        {
            FirebaseContext context = new FirebaseContext();
            var shifts = context.Client.Child("shifts");

            // Query the shifts to find the document with matching ShiftId
            var shiftQuery = shifts.OrderBy("ShiftId").EqualTo(shiftId).OnceAsync<Shift>();

            // Get the first matching shift document
            var shift = shiftQuery.Result.FirstOrDefault();

            // Delete the shift document if it exists
            if (shift != null)
            {
                context.Client.Child("shifts").Child(shift.Key).DeleteAsync();
            }

            // Redirect back to the DisplayAllShifts action
            return RedirectToAction("DisplayAllShifts", new { successMessage = true });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAllShifts()
        {
            FirebaseContext context = new FirebaseContext();
            await context.Client.Child("shifts").DeleteAsync();

            // Redirect to the DisplayAllShifts action
            return RedirectToAction("DisplayAllShifts");
        }

    }
}
