using App.DbAccess.Models;
using App.DbAccess.Services;
using App.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAccount _account;

        public AuthenticationController(IAccount account)
        {
            _account = account;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                AccountUser user = await _account.GetUserAsync(loginModel.Login, loginModel.Password);

                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Report");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(loginModel);
        }

        private async Task Authenticate(AccountUser user)
        {
            string name = user.LastName + " " + user.FirstName;

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email));
            claims.Add(new Claim("id", user.Id.ToString()));
            claims.Add(new Claim("name", name));

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Authentication");
        }
    }
}
