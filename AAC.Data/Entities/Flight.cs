using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class Flight
    {
        public int ID { get; set; }
        public int AircraftTypeID { get; set; }
        public int PilotID { get; set; }
        public int CoPilotID { get; set; }
        public string Crew { get; set; }
        public string FlightNumber { get; set; }
        public string AircraftRegistration { get; set; }
        public string Route { get; set; }
        public string Particulars { get; set; }
        public Nullable<int> PreparedByID { get; set; }
        public Nullable<int> ReviewedByID { get; set; }
        public Nullable<int> CheckedByID { get; set; }
        public Nullable<int> NotedByID { get; set; }
        public string Comments { get; set; }
        public string Passengers { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual AircraftType AircraftType { get; set; } //mapped in Flight Mapping
        public virtual User Pilot { get; set; } //mapped in Flight Mapping
        public virtual User CoPilot { get; set; } //mapped in Flight Mapping
        public virtual User PreparedBy { get; set; } //mapped in Flight Mapping
        public virtual User ReviewedBy { get; set; } //mapped in Flight Mapping
        public virtual User Checkedby { get; set; } //mapped in Flight Mapping
        public virtual User NotedBy { get; set; } //mapped in Flight Mapping
        public virtual ICollection<FlightDetail> FlightDetails { get; set; }
    }
}
