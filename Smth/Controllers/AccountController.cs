﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Smth.Data;
using Smth.Interfaces;
using Smth.Services;

namespace Smth.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuth _authService;
        private readonly IJwt _jwtService;
        private readonly IConfiguration _configuration;

        public AccountController(IAuth authService, IJwt jwtService, IConfiguration configuration)
        {
            _authService = authService;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            try
            {
                var user = _authService.Register(username, email, password);
                var token = _jwtService.GenerateToken(user);

                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                });

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _authService.Login(username, password);
            if (user != null)
            {
                var token = _jwtService.GenerateToken(user);

                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(60)
                });

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Неверный логин или пароль.";
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Login");
        }
    }
}
