using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class Destination
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<FlightDetail> FlightDetailsFrom { get; set; }
        public virtual ICollection<FlightDetail> FlightDetailsTo { get; set; }
        public virtual ICollection<Schedule> ScheduleStarts { get; set; }
        public virtual ICollection<Schedule> ScheduleDestinations { get; set; }
        public virtual ICollection<Schedule> ScheduleEnds { get; set; }
    }
}
