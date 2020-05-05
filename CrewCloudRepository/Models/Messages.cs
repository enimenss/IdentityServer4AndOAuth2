using System;
using System.Collections.Generic;

namespace CrewCloudRepository.Models
{
    public partial class Messages
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RestaurantId { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
