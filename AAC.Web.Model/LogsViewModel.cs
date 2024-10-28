using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Web.Model
{
    public class LogsViewModel
    {
        public int ID { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }
        public string ModifiedBy { get; set; }
        public string DateModified { get; set; }
    }
}
