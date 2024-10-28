using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class ScheduleCrew
    {
        public int ID { get; set; }
        public int ScheduleID { get; set; }
        public int CrewID { get; set; }

        public virtual User Crew { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
