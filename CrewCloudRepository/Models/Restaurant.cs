using System;
using System.Collections.Generic;

namespace CrewCloudRepository.Models
{
    public partial class Restaurant
    {

        public string RestaurantId { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Country { get; set; }


        public string Name { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public double UserLong { get; set; }
        public double UserLat { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

     //   public ICollection<Job> Jobs { get; set; }
       // public ICollection<RestaurantImage> Images { get; set; }
    }
}
