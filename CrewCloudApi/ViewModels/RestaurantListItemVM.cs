using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrewCloudApi.ViewModels
{
    public class RestaurantListItemVM
    {
        public string RestaurantId { get; set; }
      //  public int IdentityId { get; set; }
        public string Name { get; set; }
        //public double Long { get; set; }
        //public double Lat { get; set; }
        public DateTime Date { get; set; }
        public string City { get; set; }
        public int Distance { get; set; }
        //  public int Status { get; set; }
        //  public string Description { get; set; }
    }
}
