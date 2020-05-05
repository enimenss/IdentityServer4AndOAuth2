using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudRepository.Models
{
    public class RestaurantParameters
    {
        public string JobType { get; set; } = String.Empty;
      //  public string Paid { get; set; } = "Monthly";
        public uint MonthlyPaid { get; set; }
        public uint DailyPaid { get; set; }
        public string Country { get; set; } = String.Empty;
        public string City { get; set; } = String.Empty;
        public uint Radius { get; set; }

    }
}
