using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClientMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ClientMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return Challenge(new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                RedirectUri = "/Home/Index",

            }, "OpenIdConnect");

        }

        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var id_token = await HttpContext.GetTokenAsync("id_token");
            var claims = HttpContext.User.Claims;

            return View();
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LocalLogout(string sid)
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentSid = User.FindFirst("sid")?.Value ?? "";
                if (string.Equals(currentSid, sid, StringComparison.Ordinal))
                {
                    //Maybe SignalR to notify clients
                    //await HttpContext.SignOutAsync();
                    await HttpContext.SignOutAsync("Cookies");
                    SignOut("Cookies", "OpenIdConnect");
                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("Login");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Error(string errorId)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            return SignOut("Cookies","OpenIdConnect");
        }
    }
}
