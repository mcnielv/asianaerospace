using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Web.Model.Registration
{
    public class RegistrationViewModel
    {
        public int ID { get; set; }
        public int AircraftID { get; set; }
        public string AircraftName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Aircraft.AircraftViewModel> Aircrafts { get; set; }
    }
}
