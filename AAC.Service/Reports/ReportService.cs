using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Core;
using AAC.Data;
using AAC.Data.Entities;

using AAC.Web.Model.Reports;
using AAC.Web.Model.Scheduler;
using MNV.Infrastructure.Converter;

namespace AAC.Service.Reports
{
    public class ReportService
    {
        private AAContext _context;

        #region Constructor
        public ReportService()
        {
            _context = new AAContext();
        }
        #endregion

        #region Method(s)
        public List<RptScheduleViewModel> GetAllSchedules(string start, string end)
        {
            List<RptScheduleViewModel> models = new List<RptScheduleViewModel>();
            DateTime dtStart = DateTime.Parse( start )
                , dtEnd = DateTime.Parse( end );

            models = _context.AsQueryable<Schedule>()
                .Where( x => ( DbFunctions.TruncateTime( x.StartDate ) >= dtStart && DbFunctions.TruncateTime( x.StartDate ) <= dtEnd ) )
                .OrderBy(x=>x.StartDate)
                .Select( x => new RptScheduleViewModel()
                {
                    Date = x.StartDate.ToString(),
                    AircraftRegistration = x.AircraftType.Name + "-" + x.Registration.Name,
                    FirstLegOfFlight = x.RouteStart.Name + "-" + x.RouteDestination.Name,
                    SecondLegOfFlight = x.RouteDestination.Name + "-" + x.RouteEnd.Name,
                    StartDate = x.StartDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    StartDate2 = x.WaitingStart.ToString(),
                    EndDate2 = x.WaitingEnd.ToString(),
                    Notes = x.Notes,
                    Passenger = x.Passengers,
                    FlightInfo = x.FlightInfo,
                    TechStops = x.TechnicalStops,
                    Etc = x.ETC,
                    Pilot = x.Pilot.LastName + ", " + x.Pilot.FirstName,
                    CoPilot = x.AssistantPilot.LastName + ", " + x.AssistantPilot.FirstName,
                    Crews = x.ScheduleCrews.Where( y => y.ScheduleID == x.ID ).Select( y => new CrewViewModel()
                    {
                        Fullname = y.Crew.LastName + ", " + y.Crew.FirstName
                    } ).ToList()
                } )               
                .ToList();
            List<RptScheduleViewModel> retvalues = new List<RptScheduleViewModel>();
            foreach ( var model in models )
            {
                try
                {
                    string date = DateTime.Parse( model.Date ).ToString( "MM/dd/yyyy" );
                    DateTime s1 = DateTime.Parse( model.StartDate )
                           , s2 = DateTime.Parse( model.StartDate2 )
                           , e1 = DateTime.Parse( model.EndDate )
                           , e2 = DateTime.Parse( model.EndDate2 );

                    model.StartDate = s1.ToString( "MM/dd/yyyy" );
                    model.StartTime = s1.ToString( "HH:mm" );
                    model.EndDate = e1.ToString( "MM/dd/yyyy" );
                    model.EndTime = e1.ToString( "HH:mm" );

                    model.StartDate2 = s2.ToString( "MM/dd/yyyy" );
                    model.StartTime2 = s2.ToString( "HH:mm" );
                    model.EndDate2 = e2.ToString( "MM/dd/yyyy" );
                    model.EndTime2 = e2.ToString( "HH:mm" );

                    model.Date = date;
                    retvalues.Add( model );
                }
                catch { continue;  }
            }
            return retvalues;
        }
        public string GenerateCSV( List<RptScheduleViewModel> models, string path )
        {
            //StringBuilder sb = new StringBuilder();
            List<CSVScheduleViewModel> contents = new List<CSVScheduleViewModel>();

            //string header = "Date,Registry No.,Flight(1st leg),Waiting Time,Flight(2nd leg),Pilot,Co-Pilot,Crew(s),Notes,Passengers,Flight Info,Technical Stops,ETC";
            //Csv csv = new Csv();
            #region Loop Schedule
            contents.Add( new CSVScheduleViewModel()
            {
                Date = "Date",
                RegistryNumber = "Registry No.",
                FlightFirstLeg = "Flight(1st leg)",
                WaitingTime = "Waiting Time",
                FlightSecondLeg = "Flight(2nd leg)",
                Pilot = "Pilot",
                CoPilot = "Co-Pilot",
                AircraftCrew = "Crew(s)",
                Notes = "Notes",
                Passengers = "Passengers",
                FlightInfo = "Flight Info",
                TechnicalStops = "Technical Stops",
                ETC = "ETC"
            } );
            foreach ( var value in models )
            {
                var startend = value.StartDate + " - " + value.EndDate;
                if ( value.StartDate == value.StartDate2 )
                    startend = value.StartDate;

                var startend2 = value.EndDate2 + " - " + value.EndDate;
                if ( value.EndDate2 == value.EndDate )
                    startend2 = value.EndDate2;

                var waiting = value.StartDate2 + " - " + value.EndDate2;
                if ( value.StartDate2 == value.EndDate2 )
                    waiting = value.StartDate2;

                var startendTime = value.StartTime + " - " + value.StartTime2;
                var waitingTime = value.StartTime2 + " - " + value.EndTime2;
                var startendTime2 = value.EndTime2 + " - " + value.EndTime;
                var waitingLocation = value.FirstLegOfFlight.Split( '-' );
                var crews = string.Empty;
                foreach ( var crew in value.Crews )
                {
                    crews += crew.Fullname + "\n";
                }
                //if ( !string.IsNullOrEmpty( crews ) )
                //    crews = crews.Trim().Remove( crews.Length - 1 );

                if ( value.StartDate == value.StartDate2 )
                    startend = value.StartDate;

                var schedule = new CSVScheduleViewModel()
                {
                    Date = value.Date,
                    RegistryNumber = value.AircraftRegistration,
                    FlightFirstLeg = value.FirstLegOfFlight + string.Format( " [{0} ({1})]", startend, startendTime ),
                    WaitingTime = waitingLocation[1] + string.Format( " [{0} ({1})]", waiting, waitingTime ),
                    FlightSecondLeg = value.SecondLegOfFlight + string.Format( " [{0} ({1})]", startend2, startendTime2 ),
                    Pilot = value.Pilot,
                    CoPilot = value.CoPilot,
                    AircraftCrew = crews,
                    Notes = value.Notes,
                    Passengers = value.Passenger,
                    FlightInfo = value.FlightInfo,
                    TechnicalStops = value.TechStops,
                    ETC = value.Etc
                };
                contents.Add( schedule );
            }
            #endregion
            string filename = "";
            if ( contents != null )
            {
                CsvExport<CSVScheduleViewModel> csv = new CsvExport<CSVScheduleViewModel>( contents );
                string csvdata = csv.Export( false );


                
                if ( System.IO.File.Exists( path ) )
                    System.IO.File.Delete( path );

                if ( !System.IO.File.Exists( path ) )
                {
                    //TextWriter tw = new StreamWriter( path );
                    //tw.WriteLine( expected );
                    using ( StreamWriter sw = new StreamWriter( new FileStream( path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite ) ) )
                    {
                        sw.WriteLine( csvdata );
                    }
                }
                filename = "RptSchedule.csv";
                System.IO.File.OpenText( path );
            }
            return filename;
        }
        #endregion
    }
}
