using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Web.Model.Scheduler
{
    public class CalendarViewModel
    {
        public int LoggedUserID { get; set; }
        public int ID { get; set; }
        public string Title { get; set; }
        public int AircraftID { get; set; }
        public List<Aircraft.AircraftViewModel> Aircrafts { get; set; }
        public int RegistrationID { get; set; }
        public List<Registration.RegistrationViewModel> Registrations { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string WaitingStart { get; set; }
        public string WaitingEnd { get; set; }
        public int RouteStartID { get; set; }
        //public List<Destination.DestinationViewModel> RouteStarts { get; set; }
        public int DestinationID { get; set; }
        public List<Destination.DestinationViewModel> Destinations { get; set; }
        public int RouteEndID { get; set; }
        //public List<Destination.DestinationViewModel> RouteEnds { get; set; }
        public int PilotID { get; set; }
        public List<User.UserViewModel> Pilots { get; set; }
        public string CrewIDs { get; set; }
        public List<User.UserViewModel> Crews { get; set; }
        public List<CrewViewModel> ScheduleCrews { get; set; }
        public int CopilotID { get; set; }
        //public List<User.UserViewModel> Copilots { get; set; }
        public string Notes { get; set; }
        public string Passengers { get; set; }
        public string FlightInfo { get; set; }
        public string TechnicalStops { get; set; }
        public string ETC { get; set; }
        public bool IsChangeSchedOnly { get; set; }
        public Registration.RegistrationViewModel Registration { get; set; }
        public Aircraft.AircraftViewModel Aircraft { get; set; }
    }

    public class MyEvent
    {
        public string title;
        public string start;
        public string end;
        public int id;
        public string aircrafname;
        public string registrationname;
    }
}
