using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.IdentityModels
{
    public class IdentityUserViewModel
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public DateTime DateOfBirth { get; set;}
        public string Gender { get; set; }
        public int Phone { get; set; }

               
       // public string Contry { get; set; }
      //  public string City { get; set; }
        
       
        
        
        //public string ExtraInfo { get; set; }
      //  public string CoverLetter { get; set; }
        //public string ProfilePicture { get; set; }


    }
}
