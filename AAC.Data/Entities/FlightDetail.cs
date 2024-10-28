using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Data.Entities
{
    public class FlightDetail
    {
        public int ID { get; set; }
        public int FlightID { get; set; }
        public int FromID { get; set; }
        public int ToID { get; set; }
        public int OffBlk { get; set; }
        public int OnBlk { get; set; }
        public int BLKTime { get; set; }
        public int OffGRD { get; set; }
        public int OnGRD { get; set; }
        public int FLTTime { get; set; }
        public int WaitingTimeFrom { get; set; }
        public int WaitingTimeTo { get; set; }
        public int NumberOfCycle { get; set; }
        public int RON { get; set; }
        public int PreFLT { get; set; }
        public int PostFLT { get; set; }
        public int FOB { get; set; }
        public int BO { get; set; }
        public int REM { get; set; }
        public int SortOrder { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual Destination From { get; set; }
        public virtual Destination To { get; set; }
    }
}
