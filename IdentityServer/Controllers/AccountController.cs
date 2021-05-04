using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityServer.Enums;
using IdentityServer.ViewModels;
using IdentityServer.IdentityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using IdentityServer.AccountViewModels;
using System.Security.Claims;
using IdentityServer4.Services;
using IdentityServer.GoogleReCaptchaModels;
using IdentityServer.EmailService;

namespace IdentityServer.Controllers
{

    public class AccountController : Controller
    {




        private CustomResult customResult = new CustomResult();
        private UserManager<AppUser> _userManager { get; }

        private SignInManager<AppUser> _signInManager { get; }

        private IIdentityServerInteractionService _interaction;

        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IIdentityServerInteractionService interaction, IEmailSender emailSender)
     {

            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _emailSender = emailSender;
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


        //[HttpPost]
        //public  async Task<IActionResult> Register(/*IdentityUserViewModel user*/)
        //{
        //    IdentityUserViewModel user = new IdentityUserViewModel
        //    {
        //        DateOfBirth = DateTime.Now,
        //        Email = "flow1@gmail.com",
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

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    //var appUser = new AppUser { UserName = user.Username, Email = user.Email };

        //    //IdentityResult result = await UserMgr.CreateAsync(appUser, user.Password);



        //    return View();

        //}



        //[HttpGet]
        //public ActionResult Register1()
        //{
        //    //var appUser = new AppUser { UserName = user.Username, Email = user.Email };

        //    //IdentityResult result = await UserMgr.CreateAsync(appUser, user.Password);



        //    return new JsonResult("ohh");

        //}

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var externalProvider = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(returnUrl).GetValueOrDefault("external_provider");

            if (!string.IsNullOrEmpty(externalProvider))
            {
                //return RedirectToAction("ExternalLogin", new { provider = "Facebook", returnUrl = returnUrl });
                return ExternalLogin(externalProvider, returnUrl);
            }
         

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                   // _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                   // _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        //{
        //    // Ensure the user has gone through the username & password screen first
        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load two-factor authentication user.");
        //    }

        //    var model = new LoginWith2faViewModel { RememberMe = rememberMe };
        //    ViewData["ReturnUrl"] = returnUrl;

        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }

        //    var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        //    var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

        //    if (result.Succeeded)
        //    {
        //      //  _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else if (result.IsLockedOut)
        //    {
        //       // _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
        //        return RedirectToAction(nameof(Lockout));
        //    }
        //    else
        //    {
        //       // _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
        //        ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
        //        return View();
        //    }
        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load two-factor authentication user.");
        //    }

        //    var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

        //    var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        //    if (result.Succeeded)
        //    {
        //       // _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        //_logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
        //        return RedirectToAction(nameof(Lockout));
        //    }
        //    else
        //    {
        //       // _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
        //        ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
        //        return View();
        //    }
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {


            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {

                var captchaResponse = HttpContext.Request.Form["g-recaptcha-response"];

                var isValid = ValidateGoogleCaptchaAttribute.Validate(captchaResponse);
                if (!isValid)
                {
                    ModelState.AddModelError("Captcha","Invalid Captcha!");
                 
                    return View(model);
                }
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // _logger.LogInformation("User created a new account with password.");

                 //   var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //  _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterRestaurant(RegisterViewModel model, string returnUrl = null)
        {


            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {

                var captchaResponse = HttpContext.Request.Form["g-recaptcha-response"];

                var isValid = ValidateGoogleCaptchaAttribute.Validate(captchaResponse);
                if (!isValid)
                {
                    ModelState.AddModelError("Captcha", "Invalid Captcha!");

                    return View(model);
                }
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // _logger.LogInformation("User created a new account with password.");

                    //   var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //  _logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public bool Register(string captchaResponse,string nothing=null)
        {
            var isValid = ValidateGoogleCaptchaAttribute.Validate(captchaResponse);
            if (!isValid)
            {
                return false;
            }
             return true;
        }

        public bool RegisterRestaurant(string captchaResponse)
        {
            var isValid = ValidateGoogleCaptchaAttribute.Validate(captchaResponse);
            if (!isValid)
            {
                return false;
            }


            return true;
        }



        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            var logoutId = this.Request.Query["logoutId"].ToString();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else if (!string.IsNullOrEmpty(logoutId))
            {
                var logoutContext = await this._interaction.GetLogoutContextAsync(logoutId);
                returnUrl = logoutContext.PostLogoutRedirectUri;

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
               // _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                       // _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //        }

        //        // For more information on how to enable account confirmation and password reset please
        //        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        //        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
        //        await _emailSender.SendEmailAsync(model.Email, "Reset Password",
        //           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
        //        return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}


        [HttpGet]
        [AllowAnonymous]
        public async Task<bool> ForgotPasswordMethod(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    return false;
                }

                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user,Email);
              
                var message = new Message(new string[] { Email }, "Crew cloud password reset","Your password reset Code:"+code);
                _emailSender.SendEmail(message);
            }

            return true;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<bool> ResetPasswordMethod(ResetPasswordMethodViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return false;
            }

            var isValid = _userManager.VerifyChangePhoneNumberTokenAsync(user,model.Code,model.Email);

            if (isValid.Result == false)
            {
                return false;
            }

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

            if (passwordChangeResult.Succeeded)
            {
                return true;
            }

            return false;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string code = null)
        //{
        //    if (code == null)
        //    {
        //        throw new ApplicationException("A code must be supplied for password reset.");
        //    }
        //    var model = new ResetPasswordViewModel { Code = code };
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }
        //    var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(ResetPasswordConfirmation));
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
 }
}