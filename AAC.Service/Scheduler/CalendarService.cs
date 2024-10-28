using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Core;
using AAC.Data;
using AAC.Data.Entities;
using AAC.Service.DataManagement;
using AAC.Web.Model;
using AAC.Web.Model.Enum;
using AAC.Web.Model.Scheduler;
using AAC.Web.Model.User;

using MNV.Infrastructure.Encryption;


namespace AAC.Service.Scheduler
{
    public class CalendarService
    {
        private AAContext _context;
        private RegistrationService _regService;
        private UserService _userService;

        #region Constructor
        public CalendarService()
        {
            _context = new AAContext();
            _regService = new RegistrationService();
            _userService = new UserService();
        }
        #endregion

        #region Method(s)
        public IQueryable<CalendarViewModel> GetAll()
        {
            var data = _context.AsQueryable<Schedule>()
                .Select( x => new CalendarViewModel()
                {
                    ID = x.ID,
                    Title = x.Name,
                    AircraftID = x.AircraftID,
                    RegistrationID = x.RegistrationID,
                    Start = x.StartDate.ToString(),//"yyyy-MM-dd HH:mm"
                    End = x.EndDate.ToString(),
                    WaitingStart = x.WaitingStart.ToString(),
                    WaitingEnd = x.WaitingEnd.ToString(),
                    RouteStartID = x.RouteStartID,
                    DestinationID = x.RouteDestinationID,
                    RouteEndID = x.RouteEndID,
                    PilotID = x.PilotID,
                    CopilotID = x.AssistantPilotID,
                    Notes = x.Notes,
                    Passengers = x.Passengers,
                    FlightInfo = x.FlightInfo,
                    TechnicalStops = x.TechnicalStops,
                    ETC = x.ETC,
                    ScheduleCrews = x.ScheduleCrews.Where( y => y.ScheduleID == x.ID ).Select( z => new CrewViewModel() {
                        CrewID = z.CrewID, ScheduleID = z.ScheduleID
                    } ).ToList()
                } );
            foreach ( var cal in data )
            {
                cal.Start = DateTime.Parse( cal.Start ).ToString( "yyyy-MM-dd HH:mm" );
                cal.End = DateTime.Parse( cal.End ).ToString( "yyyy-MM-dd HH:mm" );
                if ( !string.IsNullOrEmpty( cal.WaitingStart ) )
                    cal.WaitingStart = DateTime.Parse( cal.WaitingStart ).ToString( "yyyy-MM-dd HH:mm" );
                if ( !string.IsNullOrEmpty( cal.WaitingEnd ) )
                    cal.WaitingEnd = DateTime.Parse( cal.WaitingEnd ).ToString( "yyyy-MM-dd HH:mm" );
            }
            return data;
        }
        public void CreateCrews(CrewViewModel model)
        {
            var crew = new ScheduleCrew()
            {
                CrewID = model.CrewID,
                ScheduleID = model.ScheduleID
            };
            _context.Add<ScheduleCrew>( crew );
            _context.SaveChanges();
        }
        public string Create( CalendarViewModel model )
        {
            string msg = string.Empty;
            try
            {
                int id = 0;
                var schedule = new Schedule()
                {
                    Name = model.Title,
                    AircraftID = model.AircraftID,
                    RegistrationID = model.RegistrationID,
                    StartDate = DateTime.Parse( model.Start ),
                    EndDate = DateTime.Parse( model.End ),
                    RouteStartID = model.RouteStartID,
                    RouteDestinationID = model.DestinationID,
                    RouteEndID = model.RouteEndID,
                    PilotID = model.PilotID,
                    AssistantPilotID = model.CopilotID,
                    Notes = model.Notes,
                    Passengers = model.Passengers,
                    FlightInfo = model.FlightInfo,
                    TechnicalStops = model.TechnicalStops,
                    ETC = model.ETC,
                    WaitingStart = DateTime.Parse( model.WaitingStart ),
                    WaitingEnd = DateTime.Parse( model.WaitingEnd )
                };
                _context.Add<Schedule>( schedule );
                _context.SaveChanges();
                id = schedule.ID;
                #region Create Crew data
                if ( id > 0 )
                {
                    string crewids = model.CrewIDs.Remove( model.CrewIDs.Length - 1 );
                    string[] ids = crewids.Split( ',' ); 
                    foreach( var crewid in ids)
                    {
                        if(! string.IsNullOrEmpty( crewid ) )
                        {
                            CrewViewModel crewmodel = new CrewViewModel()
                            {
                                CrewID = Convert.ToInt32( crewid ),
                                ScheduleID = id
                            };
                            this.CreateCrews( crewmodel );
                        }                     
                    }
                }
                #endregion

                #region Log Create
                string fullname = string.Empty;
                var user = _userService.GetAll().Where( x => x.ID == model.LoggedUserID ).FirstOrDefault();
                if ( user != null )
                {
                    fullname = string.Format( "{0}, {1}", user.LastName, user.FirstName );
                    LogsViewModel log = new LogsViewModel()
                    {
                        ActionType = ActionType.DataModification.ToString(),
                        Description = string.Format( "New Schedule Created. <br /> ScheduleID:{0} <br /> LoggedUserID: {1}", id, user.ID ),
                        ModifiedBy = fullname,
                        DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                    };
                    new LogsService().Create( log );
                }
                #endregion
                msg = "Schedule added.";
            }
            catch(Exception ex)
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Calendar Service Error : Create()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
            return msg;
        }
        public string Update( CalendarViewModel model )
        {
            string msg = string.Empty;
            try
            {
                #region Update Calendar Schedule
                var calendar = _context.AsQueryable<Schedule>().Where( x => x.ID == model.ID ).FirstOrDefault();
                if( calendar != null)
                {
                    if ( model.IsChangeSchedOnly == false )
                    {
                        if ( !string.IsNullOrEmpty( model.Title ) )
                            calendar.Name = model.Title;
                        if ( model.AircraftID > 0 )
                            calendar.AircraftID = model.AircraftID;
                        if ( model.RegistrationID > 0 )
                            calendar.RegistrationID = model.RegistrationID;
                        if ( !string.IsNullOrEmpty( model.Start ) )
                            calendar.StartDate = DateTime.Parse( model.Start );
                        if ( !string.IsNullOrEmpty( model.End ) )
                            calendar.EndDate = DateTime.Parse( model.End );
                        if ( model.RouteStartID > 0 )
                            calendar.RouteStartID = model.RouteStartID;
                        if ( model.DestinationID > 0 )
                            calendar.RouteDestinationID = model.DestinationID;
                        if ( model.RouteEndID > 0 )
                            calendar.RouteEndID = model.RouteEndID;
                        if ( model.PilotID > 0 )
                            calendar.PilotID = model.PilotID;
                        if ( model.CopilotID > 0 )
                            calendar.AssistantPilotID = model.CopilotID;
                        if ( !string.IsNullOrEmpty( model.Notes ) )
                            calendar.Notes = model.Notes;
                        if ( !string.IsNullOrEmpty( model.Passengers ) )
                            calendar.Passengers = model.Passengers;
                        if ( !string.IsNullOrEmpty( model.FlightInfo ) )
                            calendar.FlightInfo = model.FlightInfo;
                        if ( !string.IsNullOrEmpty( model.TechnicalStops ) )
                            calendar.TechnicalStops = model.TechnicalStops;
                        if ( !string.IsNullOrEmpty( model.ETC ) )
                            calendar.ETC = model.ETC;
                        if ( !string.IsNullOrEmpty( model.WaitingStart ) )
                            calendar.WaitingStart = DateTime.Parse( model.WaitingStart );
                        if ( !string.IsNullOrEmpty( model.WaitingEnd ) )
                            calendar.WaitingEnd = DateTime.Parse( model.WaitingEnd );

                        _context.Update( calendar );
                        _context.SaveChanges();

                        #region Update Crew
                        //delete crew first
                        this.DeleteCrewBySchedule( calendar.ID, model.LoggedUserID );
                        string crewids = model.CrewIDs.Remove( model.CrewIDs.Length - 1 );
                        string[] ids = crewids.Split( ',' );
                        foreach ( var crewid in ids )
                        {
                            if ( !string.IsNullOrEmpty( crewid ) )
                            {
                                CrewViewModel crewmodel = new CrewViewModel()
                                {
                                    CrewID = Convert.ToInt32( crewid ),
                                    ScheduleID = calendar.ID
                                };
                                this.CreateCrews( crewmodel );
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        //change sched only
                        string newStart = "", newEnd = "", updatedEnd = "", newStart2 = "", updatedStart2 = "", newEnd2 = "", updatedEnd2 = "";
                      
                        newStart = DateTime.Parse( model.Start ).ToString( "yyyy-MM-dd HH:mm" );
                        newEnd = DateTime.Parse( calendar.EndDate.ToString() ).ToString( "yyyy-MM-dd HH:mm" );
                        newStart2 = DateTime.Parse( calendar.WaitingStart.ToString() ).ToString( "yyyy-MM-dd HH:mm" );
                        newEnd2 = DateTime.Parse ( calendar.WaitingEnd.ToString() ).ToString( "yyyy-MM-dd HH:mm" );

                        #region Get diffrence
                        double diff1 = 0;
                        diff1 = calendar.StartDate.Date.Subtract( calendar.EndDate ).Days;
                        //((DateTime)calendar.WaitingEnd).Date.Subtract( calendar.StartDate.Date ).Days;
                        #endregion

                        if ( diff1 > 0 )
                            updatedEnd = DateTime.Parse( newStart ).AddDays(diff1).ToString( "yyyy-MM-dd HH:mm" ).Substring( 0, 10 ) + newEnd.Substring( 10 );
                        else
                            updatedEnd = newStart.Substring( 0, 10 ) + newEnd.Substring( 10 );

                        updatedStart2 = newStart.Substring( 0,10 ) + newStart2.Substring( 10 );
                        updatedEnd2 = newEnd2.Substring( 0,10) + newEnd2.Substring( 10 );

                        calendar.StartDate = DateTime.Parse( model.Start );
                        calendar.EndDate = DateTime.Parse( updatedEnd );
                        calendar.WaitingStart = DateTime.Parse( updatedStart2 );
                        calendar.WaitingEnd = DateTime.Parse( updatedEnd2 );

                        _context.Update( calendar );
                        _context.SaveChanges();
                    }
                
                    msg = "Schedule updated.";
                }
                #endregion

                #region Log Create
                string fullname = string.Empty;
                var user = _userService.GetAll().Where( x => x.ID == model.LoggedUserID ).FirstOrDefault();
                if ( user != null )
                {
                    fullname = string.Format( "{0}, {1}", user.LastName, user.FirstName );
                    LogsViewModel log = new LogsViewModel()
                    {
                        ActionType = ActionType.DataModification.ToString(),
                        Description = string.Format( "Schedule Updated. <br /> ScheduleID:{0} <br /> LoggedUserID: {1}", model.ID, user.ID ),
                        ModifiedBy = fullname,
                        DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                    };
                    new LogsService().Create( log );
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Calendar Service Error : Update()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
          
            return msg;
        }
        public void DeleteCrewBySchedule( int scheduleID, int loggedUserID )
        {
            string msg = string.Empty;
            try
            {
                //_context.ExecuteTSQL( string.Format( "{0}", scheduleID ) );          
                var crews = _context.AsQueryable<ScheduleCrew>().Where( x => x.ScheduleID == scheduleID ).ToList();
                if(crews.Count() > 0)
                    foreach(var crew in crews)
                    {
                        this.DeleteCrew( crew.ID );
                    }

                msg = "Schedule deleted.";
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Calendar Service Error : DeleteCrewBySchedule()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
        }
        public string DeleteCrew( int id  )
        {
            string msg = string.Empty;
            try
            {
                ScheduleCrew crew = _context.AsQueryable<ScheduleCrew>().Where( x => x.ID == id ).FirstOrDefault();
                if ( crew != null )
                {
                    _context.Remove<ScheduleCrew>( crew );
                    _context.SaveChanges();
                    msg = "Crew deleted.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Calendar Service Error : DeleteCrew()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
            return msg;
        }
        public string Delete( int id, int loggedUserID )
        {
            string msg = string.Empty;
            try
            {
                Schedule schedule = _context.AsQueryable<Schedule>().Where( x => x.ID == id ).FirstOrDefault();
                if ( schedule != null )
                {
                    _context.Remove<Schedule>( schedule );
                    _context.SaveChanges();

                    #region Log Create
                    string fullname = string.Empty;
                    var user = _userService.GetAll().Where( x => x.ID == loggedUserID ).FirstOrDefault();
                    if ( user != null )
                    {
                        fullname = string.Format( "{0}, {1}", user.LastName, user.FirstName );
                        LogsViewModel log = new LogsViewModel()
                        {
                            ActionType = ActionType.DataModification.ToString(),
                            Description = string.Format( "Schedule Deleted. <br /> ScheduleID:{0} <br /> LoggedUserID: {1}", id, user.ID ),
                            ModifiedBy = fullname,
                            DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                        };
                        new LogsService().Create( log );
                    }
                    #endregion
                    msg = "Schedule deleted.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Calendar Service Error : Delete",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
            return msg;
        }
        public List<MyEvent> GetAllEvents()
        {
            var schedules = _context.AsQueryable<Schedule>();
            var data = new List<MyEvent>();

            if(schedules!= null)
                foreach(var schedule in schedules)
                {
                    data.Add( new MyEvent()
                    {
                        id = schedule.ID,
                        start = Convert.ToDateTime( schedule.StartDate.ToString() ).ToString( "yyyy-MM-dd HH:mm:ss" ),
                        end = Convert.ToDateTime( schedule.EndDate.ToString() ).ToString( "yyyy-MM-dd HH:mm:ss" ),
                        title = schedule.Name,
                        registrationname = schedule.Registration.Name,
                        aircrafname = schedule.AircraftType.Name
                    } );
                }

            return data;
        }       
        #endregion
    }
}
