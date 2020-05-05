using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CrewCloudCMSApi.Enums;
using CrewCloudCMSApi.IdentityModels;
using CrewCloudCMSApi.ViewModels;
using CrewCloudRepository.Contracts;
using CrewCloudRepository.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrewCloudCMSApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
    

        private CustomResult customResult = new CustomResult();
        private UserManager<AppUser> UserMgr { get; }
       
        private IRepositoryWrapper _repoWrapper;   
        private SignInManager<AppUser> SignInMgr { get; }

        

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IRepositoryWrapper repoWrapper)
     {
            
            UserMgr = userManager;
            SignInMgr = signInManager;
            _repoWrapper = repoWrapper;
     }

        //[HttpGet]
        // public async Task<IActionResult> Register()
        // {
        //     return View();
        // }


        //[HttpGet]
        //public async Task<IActionResult> Login(string returnUrl)
        //{
        //    // build a model so we know what to show on the login page
        

        //    return View();
        //}


        //[HttpGet]
        //public  async Task<IActionResult> Register(/*IdentityUserViewModel user*/)
        //{
        //    var test = User.Identity.Name;

        //    IdentityUserViewModel user = new IdentityUserViewModel
        //    {
        //        DateOfBirth = DateTime.Now,
        //        Email = "flow5666@gmail.com",
        //        FirstName = "omg23",
        //        Gender = "eh",
        //        LastName = "help",
        //        Phone = 321,
        //        Password = "Flow!0707"

        //    };

        //    //var options = new JsonSerializerOptions
        //    //{
        //    //    AllowTrailingCommas = true
        //    //};
        //    //var json = this.Request.Form.Keys.First();
        //    //var user1= JsonSerializer.Deserialize<IdentityUserViewModel>(json, options);
        //    try
        //    {
              
        //            if (user == null)
        //            {
        //                customResult.Message = "User not found";
        //                customResult.StatusCode = ResultStatus.Error.ToString();
        //                return new JsonResult(customResult);
        //            }
        //            if (_repoWrapper.User.GetUserById(user.Email) != null)
        //            {
        //                customResult.Message = $"User with email {user.Email} already exists";
        //                customResult.StatusCode = ResultStatus.Error.ToString();
        //                return new JsonResult(customResult);
        //            }
               
        //        var appUser = new AppUser { UserName = user.Email, Email = user.Email };

        //        IdentityResult result = await UserMgr.CreateAsync(appUser, user.Password);

        //        User userRepo = new User
        //        {
        //          //  City = user.City,
        //            UserName = user.Email,
        //          //  Contry = user.Contry,
        //            LastName = user.LastName,
        //            Gender = user.Gender,
        //            FirstName = user.FirstName,
        //            Email = user.Email,
        //           // CoverLetter = user.CoverLetter,
        //            PhoneNumber = user.Phone,
        //            DateOfBirth = user.DateOfBirth,
        //            ProfilPictureId = 10,

        //        };
        //        await UserMgr.AddToRoleAsync(appUser, "User");

        //        var today = DateTime.Today;
        //        var age = today.Year - user.DateOfBirth.Year;
        //        if (user.DateOfBirth.Date > today.AddYears(-age))
        //            age--;
        //        List<UserImage> images = new List<UserImage>();
        //        images.Add(_repoWrapper.UserImage.GetImageById(10));
        //        var profileEdit = new ProfileScreenEditVM
        //        {
        //            Status = false,
        //            City = "",
        //            Contry = "",
        //            CoverLetter = "",
        //            Email = userRepo.Email,
        //            FirstName = userRepo.FirstName,
        //            LastName = userRepo.LastName,
        //            Video = "",
        //            Years = age,
        //            Images = images
        //        };


        //        _repoWrapper.User.Create(userRepo);
        //        _repoWrapper.Save();
        //        customResult.Data = profileEdit;
        //        customResult.Message = "User Registered";
        //        customResult.StatusCode = ResultStatus.Ok.ToString();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        customResult.Message = ex.Message;
        //        customResult.StatusCode = ResultStatus.Error.ToString();
        //        //   result.Message = "User is not created!";
        //    }


        //    return new JsonResult(customResult);

        //}





        public ActionResult Register1()
        {
            var test = User.Identity.Name;
            var ok = User.IsInRole("Admincina");
            var ok1 = User.IsInRole("Ok");
            var ok2 = User.IsInRole("Restaurant");

            return new JsonResult("ohh");

        }
    }
}