using IdentityServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.ViewModels
{
    public class CustomResult
    {
        public string Message { get; set; }
        public string StatusCode { get; set; }

        public object Data { get; set; }

        public CustomResult()
        {

        }

        public CustomResult(string message,ResultStatus status,object data)
        {
            Message = message;
            StatusCode = status.ToString();
            Data = data;
        }

        public static Microsoft.AspNetCore.Mvc.JsonResult Error(string message)
        {
            CustomResult result = new CustomResult
            {
                Message = message,
                StatusCode = ResultStatus.Error.ToString(),
            };

            return new Microsoft.AspNetCore.Mvc.JsonResult(result);
        }
    }
}
