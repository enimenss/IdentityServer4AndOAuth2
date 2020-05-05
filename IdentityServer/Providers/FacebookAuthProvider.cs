using IdentityServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using IdentityServer.Repositories.Interfaces;
using IdentityServer.Entities;
using IdentityServer.Helpers;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Providers
{
    public class FacebookAuthProvider<TUser> : IFacebookAuthProvider where TUser:IdentityUser<int> , new()
    {
 
        private readonly IProviderRepository _providerRepository;
        private readonly HttpClient _httpClient;
        public FacebookAuthProvider(         
            IProviderRepository providerRepository,
            HttpClient httpClient
            )
        {        
            _providerRepository = providerRepository;
            _httpClient = httpClient;
        }

        public Provider Provider => _providerRepository.Get()
                                    .FirstOrDefault(x => x.Name.ToLower() == ProviderType.Facebook.ToString().ToLower());

        public JObject GetUserInfo(string accessToken)
        {
            if(Provider == null)
            {
                throw new ArgumentNullException(nameof(Provider));
            }

            var request = new Dictionary<string, string>();

            request.Add("fields", "id,email,name,gender,birthday,picture.width(480).height(480)");
            request.Add("access_token", accessToken);

            var result = _httpClient.GetAsync(Provider.UserInfoEndPoint + QueryBuilder.GetQuery(request, ProviderType.Facebook)).Result;
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return infoObject;
            }
            return null;
        }
    }
}
