using IdentityServer.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class UserRolesVM
    {
       public AppUser user { get; set; }
       public IList<string> roles { get; set; }
    }
}
