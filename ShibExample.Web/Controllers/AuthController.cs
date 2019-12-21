using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShibExample.Web.Auth;

namespace ShibExample.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly AppSettings _appSettings;

        public AuthController(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }
        /// <summary>
        /// Login View, this is the main entry point for establishing an identity,
        /// in the case that Shib is disabled, you'll set the user object here.
        /// </summary>
        /// <returns></returns>
        public IActionResult Login(string ReturnUrl)
        {
            IShibClaim shib = new ShibClaim();
            ClaimsPrincipal principal;

            if (this._appSettings.EnableShib)
            {
                principal = shib.BuildClaimsPrincipal(HttpContext.Request.Headers["YourUsernameHeaderValue"]);
            }
            else
            {
                // If you don't want to use shib you can force a stub username into the system here.
                // We do this locally when we arent' running through shibb during development.
                principal = shib.BuildClaimsPrincipal("static username");
            }

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(ReturnUrl);
        }

        /// <summary>
        /// This is a formamlity,  In reality, we can't log the user
        /// out of shib.
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult UnAuthorized()
        {
            return View();
        }
    }
}