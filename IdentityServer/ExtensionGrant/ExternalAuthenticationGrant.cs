using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdentityServer.Entities;
using IdentityServer.Helpers;
using IdentityServer.Interfaces;
using IdentityServer.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ExtensionGrant
{
    public class ExternalAuthenticationGrant<TUser> : IExtensionGrantValidator where TUser : IdentityUser<int>, new()
    {
        private readonly UserManager<TUser> _userManager;      
        private readonly IProviderRepository _providerRepository;
        private readonly IFacebookAuthProvider _facebookAuthProvider;
        private readonly IGoogleAuthProvider _googleAuthProvider;

        public ExternalAuthenticationGrant(
            UserManager<TUser> userManager,            
            IProviderRepository providerRepository,
            IFacebookAuthProvider facebookAuthProvider,
            IGoogleAuthProvider googleAuthProvider
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));            
            _providerRepository = providerRepository?? throw new ArgumentNullException(nameof(providerRepository));
            _facebookAuthProvider = facebookAuthProvider ?? throw new ArgumentNullException(nameof(facebookAuthProvider));
            _googleAuthProvider = googleAuthProvider ?? throw new ArgumentNullException(nameof(googleAuthProvider));

            _providers = new Dictionary<ProviderType, IExternalAuthProvider>
            {
                 {ProviderType.Facebook, _facebookAuthProvider},
                 {ProviderType.Google, _googleAuthProvider},

            };
        }


        private Dictionary<ProviderType, IExternalAuthProvider> _providers;
        
        public string GrantType => "external";
       


        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var provider = context.Request.Raw.Get("provider");
            if (string.IsNullOrWhiteSpace(provider))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }

            
            var token = context.Request.Raw.Get("external_token");
            if(string.IsNullOrWhiteSpace(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid external token");
                return;
            }

            var requestEmail = context.Request.Raw.Get("email"); 

            var providerType=(ProviderType)Enum.Parse(typeof(ProviderType), provider,true);

            if (!Enum.IsDefined(typeof(ProviderType), providerType))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "invalid provider");
                return;
            }

            var userInfo = _providers[providerType].GetUserInfo(token);

            if(userInfo == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "couldn't retrieve user info from specified provider, please make sure that access token is not expired.");
                return;
            }
            string externalId = string.Empty;
            if (providerType == ProviderType.Facebook)
            {
                externalId = userInfo.Value<string>("id");
            }
            else if (providerType == ProviderType.Google)
            {
                externalId = userInfo.Value<string>("sub");
            }
            if (!string.IsNullOrWhiteSpace(externalId))
            {
               
                var user = await _userManager.FindByLoginAsync(provider, externalId);
                if(null != user)
                {
                    user = await _userManager.FindByIdAsync(user.Id.ToString());
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    context.Result = new GrantValidationResult(user.Id.ToString(), provider, userClaims, provider, null);
                    return;
                }
                else
                {
                    var newUser = new TUser { Email = userInfo.Value<string>("email"), UserName = userInfo.Value<string>("email") };
                    var result = await _userManager.CreateAsync(newUser);
                    if (result.Succeeded)
                    {
                        await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, externalId, provider));
                        var userClaims = await _userManager.GetClaimsAsync(newUser);
                        context.Result = new GrantValidationResult(newUser.Id.ToString(), provider, userClaims, provider, null);
                    }
                    else
                    {
                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "user could not be created, please try again");
                    }
                }
            }

           


            return;
        }
    }
}
