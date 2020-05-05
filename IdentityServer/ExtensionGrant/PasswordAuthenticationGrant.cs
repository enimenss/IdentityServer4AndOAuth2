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

   
    public class PasswordAuthenticationGrant<TUser> : IResourceOwnerPasswordValidator where TUser : IdentityUser<int>, new()
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;

        public PasswordAuthenticationGrant(
             UserManager<TUser> userManager,
             SignInManager<TUser> signInManager
            )
           {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

 

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var username = context.UserName;
            var password = context.Password;
            var user = _userManager.FindByNameAsync(username);

            if (user != null && user.Result != null)
            {
               // var test=await _signInManager.PasswordSignInAsync(username, password, false, lockoutOnFailure: true); for lock
               

                var validCredentials =await _signInManager.UserManager.CheckPasswordAsync(user.Result, password);
                if (validCredentials)
                {
                    
                    var userClaims = new List<Claim>(); //await _userManager.GetClaimsAsync(userr);
                    userClaims.Add(new Claim("name", username));
                    List<string> roles = _userManager.GetRolesAsync(user.Result).Result.ToList();
                    foreach(var role in roles)
                    {
                        userClaims.Add(new Claim("role",role));
                    }
                    context.Result = new GrantValidationResult(user.Result.Id.ToString(), "password", userClaims);
                    return;
                }
            }
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "wrong user, please try again");
            

        }

    }
}
