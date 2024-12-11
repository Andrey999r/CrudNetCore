using Smth.Data;
using Microsoft.AspNetCore.Mvc;

namespace Smth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("Register", "Account"); 

            var surveys = _context.Surveys
                .Where(s => s.ApplicationUserId == userId.Value)
                .ToList();

            return View(surveys);
        }

        private int? GetCurrentUserId()
        {
            if (HttpContext.Session.TryGetValue("UserId", out byte[] userIdBytes))
            {
                return BitConverter.ToInt32(userIdBytes, 0);
            }
            return null;
        }
    }
}