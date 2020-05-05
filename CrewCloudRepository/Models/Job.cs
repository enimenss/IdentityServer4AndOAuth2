using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrewCloudRepository.Models
{
    [Table("Job")]
    public partial class Job
    {
        public int Id { get; set; }
        //  public string UserId { get; set; }
        //public string RestaurantId { get; set; }
        //   public DateTime DateFrom { get; set; }
        public DateTime Date { get; set; }
        public string JobType { get; set; }

        public string Segment { get; set; }
        public double DailyPaid{get;set;}
        public double HourlyPaid{get;set;}
        public double MonthlyPaid{get;set;}

       // [ForeignKey(nameof(Restaurant))]
        public string RestaurantId { get; set; }
     //   public Restaurant Restaurant { get; set; }
    }
}
