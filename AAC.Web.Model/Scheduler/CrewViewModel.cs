using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Web.Model.Scheduler
{
    public class CrewViewModel
    {
        public int ID { get; set; }
        public int ScheduleID { get; set; }
        public int CrewID { get; set; }
        public string Fullname { get; set; }
    }
}
