using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace IdentityServer.GoogleReCaptchaModels
{
    public static class ValidateGoogleCaptchaAttribute
    {
        public static bool Validate(string captchaResponse)
        {
            const string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            string secretKey = SiteSettings.GoogleRecaptchaSecretKey;
            

            if (string.IsNullOrWhiteSpace(captchaResponse)) return false;

            var validateResult = ValidateFromGoogle(urlToPost, secretKey, captchaResponse);
            if (!validateResult.Success) return false;

            return validateResult.Success;

        }


        private static ReCaptchaResponse ValidateFromGoogle(string urlToPost, string secretKey, string captchaResponse)
        {
            var postData = "secret=" + secretKey + "&response=" + captchaResponse;

            var request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                streamWriter.Write(postData);

            string result;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                    result = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<ReCaptchaResponse>(result);
        }
    }

    internal class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}