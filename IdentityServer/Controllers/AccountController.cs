using System.Threading.Tasks;
using IdentityServer.IdentityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using IdentityServer.AccountViewModels;
using IdentityServer4.Services;

namespace IdentityServer.Controllers
{

    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }

        private IIdentityServerInteractionService _interaction;


        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IIdentityServerInteractionService interaction)
         {

                _userManager = userManager;
                _signInManager = signInManager;
                _interaction = interaction;
         }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);        

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
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
 
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var logoutId = this.Request.Query["logoutId"].ToString();

            ViewData["LogoutId"] = logoutId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logoutContext = await this._interaction.GetLogoutContextAsync(logoutId);
            var returnUrl = logoutContext.PostLogoutRedirectUri;

            await _signInManager.SignOutAsync();
            return this.Redirect(returnUrl);

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