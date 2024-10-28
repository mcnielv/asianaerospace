using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Web.Model.Reports
{
    public class CSVScheduleViewModel
    {
        //"Date,Registry No.,Flight(1st leg),Waiting Time,Flight(2nd leg),Pilot,Co-Pilot,Crew(s),Notes,Passengers,Flight Info,Technical Stops,ETC"
        public string Date { get; set; }
        public string RegistryNumber { get; set; }
        public string FlightFirstLeg { get; set; }
        public string WaitingTime { get; set; }
        public string FlightSecondLeg { get; set; }
        public string Pilot { get; set; }
        public string CoPilot { get; set; }
        public string AircraftCrew { get; set; }
        public string Notes { get; set; }
        public string Passengers { get; set; }
        public string FlightInfo { get; set; }
        public string TechnicalStops { get; set; }
        public string ETC { get; set; }
    }
}
