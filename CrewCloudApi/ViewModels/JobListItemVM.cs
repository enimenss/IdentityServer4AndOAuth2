using System;
using System.Collections.Generic;
using System.Text;

namespace CrewCloudApi.ViewModels
{
    public class JobListItemVM
    {
        public int Id { get; set; }

       // public DateTime Date { get; set; }
        public string JobType { get; set; }

        public double DailyPaid { get; set; }
        public double HourlyPaid { get; set; }
        public double MonthlyPaid { get; set; }


     //   public string RestaurantId { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
    }
}
