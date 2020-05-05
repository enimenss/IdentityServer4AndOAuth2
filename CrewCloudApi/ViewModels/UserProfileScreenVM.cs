using CrewCloudRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrewCloudApi.ViewModels
{
    public class UserProfileScreenVM
    {
           public string Email { get; set; }
        //     public string UserName { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public DateTime DateOfBirth { get; set; }
    //    public string Image { get; set; }
        public string Video { get; set; }
        public bool Status { get; set; }
       // public string ExtraInfo { get; set; }
        public string CoverLetter { get; set; }

        public int Years { get; set; }
        public List<UserImage> Images { get; set; }
        public string Contry { get; set; }
        public string City { get; set; }

    }
}
