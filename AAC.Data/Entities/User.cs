using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class User
    {
        public int ID { get; set; }
        public int RoleID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string SSS { get; set; }
        public string TIN { get; set; }
        public bool Active { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Flight> PilotFlights { get; set; }
        public virtual ICollection<Flight> CoPilotFlights { get; set; }
        public virtual ICollection<Flight> PreparedByFlights { get; set; }
        public virtual ICollection<Flight> ReviewedByFlights { get; set; }
        public virtual ICollection<Flight> CheckedByFlights { get; set; }
        public virtual ICollection<Flight> NotedByFlights { get; set; }
        public virtual ICollection<Schedule> PilotSchedules { get; set; }
        public virtual ICollection<Schedule> AssistantPilotSchedules { get; set; }
        public virtual ICollection<ScheduleCrew> Crews { get; set; }
    }
}
