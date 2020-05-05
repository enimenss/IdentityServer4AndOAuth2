using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrewCloudRepository.Models
{
    public partial class User
    {
        [Key]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public string Contry { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public int PhoneNumber { get; set; }
        public string Video { get; set; }
        public bool Status { get; set; }
        public string ExtraInfo { get; set; }
        public string CoverLetter { get; set; }
        public string Profession { get; set; }
        public int ProfilPictureId { get; set; }
        
    }
}
