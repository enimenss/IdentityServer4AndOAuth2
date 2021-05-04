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
			//List<string> roles = _userManager.GetRoles(userId).ToList();
			context.IssuedClaims.AddRange(context.Subject.Claims);

		}

		public async Task IsActiveAsync(IsActiveContext context)
		{
			context.IsActive =true;
			
		}
	}
}
