using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class Schedule
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AircraftID { get; set; }
        public int RegistrationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RouteStartID { get; set; }
        public int RouteDestinationID { get; set; }
        public int RouteEndID { get; set; }
        public int PilotID { get; set; }
        public int AssistantPilotID { get; set; }
        public string Notes { get; set; }
        public string Passengers { get; set; }
        public string FlightInfo { get; set; }
        public string TechnicalStops { get; set; }
        public string ETC { get; set; }
        public DateTime? WaitingStart { get; set; }
        public DateTime? WaitingEnd { get; set; }

        public virtual Registration Registration { get; set; }
        public virtual AircraftType AircraftType { get; set; }
        public virtual Destination RouteStart { get; set; }
        public virtual Destination RouteDestination { get; set; }
        public virtual Destination RouteEnd { get; set; }
        public virtual User Pilot { get; set; }
        public virtual User AssistantPilot { get; set; }
        public virtual ICollection<ScheduleCrew> ScheduleCrews { get; set; }
    }
}
