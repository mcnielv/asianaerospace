using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Web.Model.Reports;
namespace AAC.Framework.IService.Reports
{
    public interface IReportService
    {
        List<RptScheduleViewModel> GetAllSchedules( string start, string end );
        string GenerateCSV( List<RptScheduleViewModel> models, string path );
    }
}
