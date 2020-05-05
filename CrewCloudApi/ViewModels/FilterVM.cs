using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrewCloudApi.ViewModels
{
    public class FilterVM
    {
        public List<string> JobTypes { get; set; }
        public List<string> Cities { get; set; }
        public List<string> Countries { get; set; }

        public List<int> Radiuses { get; set; }
       // public List<double> MonthlyPaids { get; set; }
        //public List<double> DailyPaids { get; set; }
    }
}
