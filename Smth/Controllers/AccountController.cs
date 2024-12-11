using Smth.Data;

namespace Smth.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Register(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return View();
        }

        var user = new ApplicationUser { Username = username, PasswordHash = password }; // Для примера без хеша
        _context.Users.Add(user);
        _context.SaveChanges();

        HttpContext.Session.Set("UserId", System.BitConverter.GetBytes(user.Id));

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
        if (user != null)
        {
            HttpContext.Session.Set("UserId", System.BitConverter.GetBytes(user.Id));
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserId");
        return RedirectToAction("Login");
    }
}
