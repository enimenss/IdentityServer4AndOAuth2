using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrewCloudRepository.Models
{
    
    public partial class RestaurantImage
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string Picture { get; set; }
       // [ForeignKey(nameof(Restaurant))]
        public string RestaurantId { get; set; }
        // public Restaurant Restaurant { get; set; }
        public string ImageUrl { get; set; }


    }
}
