using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class AircraftTypeRegistration
    {
        public int AircraftTypeID { get; set; }
        public int RegistrationID { get; set; }
        
        public virtual Registration Registration { get; set; } 
        public virtual AircraftType AircraftType { get; set; }
    }
}
