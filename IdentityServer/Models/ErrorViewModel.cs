using IdentityServer4.Models;
using System;

namespace IdentityServer.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public ErrorMessage Error { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
