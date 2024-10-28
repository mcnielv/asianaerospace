using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Web.Model.Reports
{
    public class RptScheduleViewModel
    {
        public string Date { get; set; }
        public string AircraftRegistration { get; set; }
        public string FirstLegOfFlight { get; set; }
        public string SecondLegOfFlight { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartDate2 { get; set; }
        public string EndDate2 { get; set; }
        public string StartTime2 { get; set; }
        public string EndTime2 { get; set; }
        public string Notes { get; set; }
        public string Passenger { get; set; }
        public string FlightInfo { get; set; }
        public string TechStops { get; set; }
        public string Etc { get; set; }
        //public string FirstLegStartEnd { get; set; }
        //public string SecondLegStartEnd { get; set; }
        //public string WaitingTime { get; set; }
        public string Pilot { get; set; }
        public string CoPilot { get; set; }
        public List<Scheduler.CrewViewModel> Crews { get; set; }
    }
}
