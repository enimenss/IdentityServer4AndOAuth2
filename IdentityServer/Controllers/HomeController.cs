using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityServer.Models;
using IdentityServer.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using IdentityServer.EmailService;

namespace IdentityServer.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //private readonly RoleManager<AppRole> _roleManager;

        //private readonly UserManager<AppUser> _userManager;


        //public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        //{
        //    _logger = logger;
        //    _roleManager = roleManager;
        //    _userManager = userManager;
        //}

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailSender _emailSender;

        public HomeController(IIdentityServerInteractionService interaction, IHostingEnvironment environment, ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _interaction = interaction;
            _environment = environment;
            _emailSender= emailSender;
        }

        public IActionResult Index()
        {
            //var message = new Message(new string[] { "jocaenimen@gmail.com" }, "Test email", "This is the content from our email.");
            //_emailSender.SendEmail(message);
            return View();
        }
        //public IActionResult Privacy()
        //{

        //    return View();
        //}

        //public IActionResult Roles()
        //{
        //    var roles =  _roleManager.Roles.ToList();
        //    var users = _userManager.Users.ToList();

        //    List<UserRolesVM> vms = new List<UserRolesVM>();
        //    foreach (var item in users)
        //    {
        //        List<string> u = _userManager.GetRolesAsync(item).Result.ToList();

        //        UserRolesVM pom = new UserRolesVM
        //        {
        //            user = item,
        //            roles = u
        //        };
        //        vms.Add(pom);



        //    }
        //    return View(vms);
        //}


        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return View("Error", vm);
        }
    }
}
