using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientMVC.Models
{
    public class PrivacyViewModel
    {
        public string IdToken { get; set; }
        public string Sub { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
