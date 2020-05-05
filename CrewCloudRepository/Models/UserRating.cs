using System;
using System.Collections.Generic;

namespace CrewCloudRepository.Models
{
    public partial class UserRating
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RestaurantId { get; set; }
        public DateTime Date { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
    }
}
