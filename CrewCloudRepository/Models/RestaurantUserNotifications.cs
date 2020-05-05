using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Models
{
    public class RestaurantUserNotifications
    {
        public int Id { get; set; }
        public string RestaurantEmail { get; set; }
        public string UserEmail { get; set; }
        public string JobTitle { get; set; }
        public DateTime Date { get; set; }
        public bool RestaurantVis { get; set; }
        public bool UserVis { get; set; }
    }
}
