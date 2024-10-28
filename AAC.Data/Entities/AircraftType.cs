using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class AircraftType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
