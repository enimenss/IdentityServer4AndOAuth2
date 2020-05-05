using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrewCloudApi.ViewModels
{
    public class MessageProfileScreenVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RestaurantId { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
