using IdentityServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Entities;
using Newtonsoft.Json.Linq;
using IdentityServer.Repositories.Interfaces;
using System.Net.Http;
using IdentityServer.Helpers;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Providers
{
    public class GoogleAuthProvider<TUser> : IGoogleAuthProvider where TUser:IdentityUser<int>,new()
    {
        
        private readonly IProviderRepository _providerRepository;
        private readonly HttpClient _httpClient;
        public GoogleAuthProvider(
             
             IProviderRepository providerRepository,
             HttpClient httpClient
             )
        {
            
            _providerRepository = providerRepository;
            _httpClient = httpClient;
        }
        public Provider Provider => _providerRepository.Get()
                                    .FirstOrDefault(x => x.Name.ToLower() == ProviderType.Google.ToString().ToLower());
        public JObject GetUserInfo(string accessToken)
        {
            var request = new Dictionary<string, string>();
            request.Add("token", accessToken);

            // var result = _httpClient.GetAsync(Provider.UserInfoEndPoint + QueryBuilder.GetQuery(request, ProviderType.Google)).Result;
            var result = _httpClient.GetAsync("https://oauth2.googleapis.com/tokeninfo?id_token=" + accessToken).Result;
           // var result = _httpClient.GetAsync("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=" + accessToken).Result;
            if (result.IsSuccessStatusCode)
            {
                var infoObject = JObject.Parse(result.Content.ReadAsStringAsync().Result);
                return infoObject;
            }
            return null;
        }
    }
}
