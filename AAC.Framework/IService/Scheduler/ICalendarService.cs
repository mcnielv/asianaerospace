using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Web.Model.Scheduler;

namespace AAC.Framework.IService.Scheduler
{
    public interface ICalendarService
    {
        IQueryable<CalendarViewModel> GetAll();
        void CreateCrews( CrewViewModel model );
        string Create( CalendarViewModel model );
        string Update( CalendarViewModel model );
        void DeleteCrewBySchedule( int scheduleID, int loggedUserID );
        string DeleteCrew( int id );
        string Delete( int id, int loggedUserID );
        List<MyEvent> GetAllEvents();
    }
}
