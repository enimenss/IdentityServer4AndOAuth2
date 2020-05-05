﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CrewCloudMVC.Models;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace CrewCloudMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;



        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string code)
        {
            //return Challenge(new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            //{
            //    RedirectUri = "/Home/Index",

            //}, "OpenIdConnect");

            return Redirect("https://localhost:44323/connect/authorize?client_id=Test&response_type=code&redirect_uri=http://localhost:5012/Home/Login&scope=openid profile custom.profile CrewCloudApi CrewCloudCMSApi");
        }


        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken =await HttpContext.GetTokenAsync("refresh_token");
            var id_token = await HttpContext.GetTokenAsync("id_token");

            var user = User.Identity.Name;

          //  var picture = User.Claims.Where(x=>x.);
            var rola1 = User.IsInRole("Test");
            var rola2 = User.IsInRole("Ok");
            var rola3 = User.IsInRole("Restaurant");


            var client = new HttpClient();


            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var apiResponse = await client.GetAsync("https://localhost:44386/weatherforecast");

            var result = apiResponse.Content.ReadAsStringAsync();

            var apiResponse1 = await client.GetAsync("https://localhost:44373/account");

            var result1 = apiResponse1.Content.ReadAsStringAsync();


            return View();
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


        public IActionResult Logout()
        {
            return SignOut("Cookies","OpenIdConnect");
        }
    }
}
