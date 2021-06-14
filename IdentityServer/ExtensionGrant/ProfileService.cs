using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ExtensionGrant
{
	public class ProfileService<TUser> : IProfileService where TUser : IdentityUser<int>, new()
	{
		private readonly UserManager<TUser> _userManager;

		public ProfileService(UserManager<TUser> userManager)
		{
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}
		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
            if (context.Caller.Equals("ClaimsProviderIdentityToken"))
            {
				//Add claims to identity token
				if(context.RequestedResources.Resources.IdentityResources.Any(x => x.Name == "openid"))
                {
					var subClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
					context.IssuedClaims.Add(subClaim);
                }
				if (context.RequestedResources.Resources.IdentityResources.Any(x => x.Name == "email"))
				{
					var emailClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "email");
					context.IssuedClaims.Add(emailClaim);
				}
				if (context.RequestedResources.Resources.IdentityResources.Any(x => x.Name == "custom.profile"))
				{
					var subClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
					var usernameClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "username");
					var emailClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "email");
					context.IssuedClaims.Add(subClaim);
					if (usernameClaim != null)
					{
						context.IssuedClaims.Add(usernameClaim);
					}
					context.IssuedClaims.Add(emailClaim);
				}

			}

			if (context.Caller.Equals("UserInfoEndpoint"))
			{
				var subClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
				if(subClaim != null)
                {
					var user = await _userManager.FindByIdAsync(subClaim.Value);
					if(user != null)
                    {
						context.IssuedClaims = context.Subject.Claims.ToList();
						var claims = await _userManager.GetClaimsAsync(user);
						context.IssuedClaims.AddRange(claims);
                    }
                }
				//return user info claims
			}

			if (context.Caller.Equals("ClaimsProviderAccessToken"))
			{
				context.IssuedClaims.AddRange(context.Subject.Claims);
				//Add claims to access token
			}



		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			var subClaim = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");
			if (subClaim == null)
			{
				context.IsActive = false;
				return;
			}
			var user = await _userManager.FindByIdAsync(subClaim.Value);

			if (user == null)
			{
				context.IsActive = false;
				return;
			}
			
			context.IsActive =true;		
		}
	}
}
