using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrewCloudRepository.Models
{
    [Table("UserImage")]
    public partial class UserImage
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Picture { get; set; }
        public DateTime Date { get; set; }                
        
       
        public string UserId { get; set; }
        public bool IsProfile { get; set; }
       
    }
}
