using System;
using System.Collections.Generic;

namespace CrewCloudRepository.Models
{
    public partial class JobPost
    {
        public int Id { get; set; }
        public string RestaurantId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string JobType { get; set; }
        public int Price { get; set; }
        public string Nenad { get; set; }
    }
}
