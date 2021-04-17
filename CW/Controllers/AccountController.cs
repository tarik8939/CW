using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CW.Models;
using CW.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CW.Controllers
{
    public class AccountController : Controller
    {
        private CWContext db;

        public AccountController(CWContext db)
        {
            this.db = db;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Worker worker = await db.Workers.Include(x=>x.Role).
                    FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (worker != null)
                {
                    await Authenticate(worker.Email, worker.Role.Role1); // аутентифікація

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            //return View(model);
            return View("../Home/Index", model);
        }
        private async Task Authenticate(string Email, string Role)
        {
            // створюємо claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,Role),
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // встановлюємо аутентифікаційні кукі 
 
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //return RedirectToAction("Login", "Account");
            return Redirect("~/Home/Index");
        }
    }
}
