using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.ExtensionGrant
{
    public class LoginAuthenticationGrant<TUser> : IExtensionGrantValidator where TUser : IdentityUser<int>, new()
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;

        public LoginAuthenticationGrant(
             UserManager<TUser> userManager,
             SignInManager<TUser> signInManager
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public string GrantType => "myLogin";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var username = context.Request.Raw.Get("username");
            var password = context.Request.Raw.Get("password");
            var user = _userManager.FindByNameAsync(username);

            if (user != null && user.Result != null)
            {
                var validCredentials = _signInManager.UserManager.CheckPasswordAsync(user.Result, password);
                if (validCredentials != null && validCredentials.Result)
                {
                    var userr = await _userManager.FindByIdAsync(user.Result.Id.ToString());
                    var userClaims = new List<Claim>(); //await _userManager.GetClaimsAsync(userr);
                    userClaims.Add(new Claim("email",username));
                    userClaims.Add(new Claim("name", username));
                    userClaims.Add(new Claim("role", "Ok"));
                    context.Result = new GrantValidationResult(user.Result.Id.ToString(), "password", userClaims,"password",null);
                    return;
                }
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidTarget, "wrong user, please try again");


        }


    }
}
