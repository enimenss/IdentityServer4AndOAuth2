using System;
using System.Collections.Generic;

namespace CrewCloudRepository.Models
{
    public partial class News
    {
        public int Id { get; set; }
        public string RestaurantId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string News1 { get; set; }
    }
}
