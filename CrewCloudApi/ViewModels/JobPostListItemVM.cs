using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudApi.ViewModels
{
    public class JobPostListItemVM
    {
       // public string RestaurantId { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
      
        // public double Long { get; set; }
        //public double Lat { get; set; }
        //public int Status { get; set; }
        //public string Adress { get; set; }
        //public DateTime Date { get; set; }
        //public string Description { get; set; }
        public int Distance { get; set; }
        public int Id { get; set; }
    }
}
