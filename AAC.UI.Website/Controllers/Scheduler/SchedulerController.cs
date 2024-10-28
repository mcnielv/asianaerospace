using System;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using AAC.Service.DataManagement;
using AAC.Web.Model;
using AAC.Web.Model.Enum;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using AAC.Web.Model.Scheduler;
using AAC.Service.Destination;
using AAC.Web.Model.User;
using AAC.Service.Scheduler;

namespace AAC.UI.Website.Controllers.Scheduler
{
    [Authorize]
    public class SchedulerController : Controller
    {

        #region Local Variable(s)
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

        private DestinationService _destService;
        private UserService _userService;
        private RegistrationService _regService;
        private AircraftTypeService _actypeService;
        private CalendarService _calService;

        #region Constructor
        public SchedulerController()
        {
            _destService = new DestinationService();
            _userService = new UserService();
            _regService = new RegistrationService();
            _actypeService = new AircraftTypeService();
            _calService = new CalendarService();
            //_userID = Convert.ToInt32( User.Identity.GetUserId() );
        }
        public SchedulerController( ApplicationUserManager userManager )
        {
            _userManager = userManager;
            var user = UserManager.FindById( Convert.ToInt32( User.Identity.GetUserId() ) );
            if ( user != null )
            {
                if ( user.RoleID != Convert.ToInt32( AAC.Web.Model.Enum.UserRole.Pilot ) )
                {
                    RedirectToAction( "LogOff", "Account" );
                }
            }
        }
        #endregion

        #region ActionResult(s)
        public ActionResult Calendar()
        {
            var users = _userService.GetAll();
            CalendarViewModel model = new CalendarViewModel();
            model.Aircrafts = _actypeService.GetAll().ToList();
            model.Pilots = users.Where( x => x.RoleID == (int) UserRole.Pilot ).ToList();
            model.Crews = users.Where( x => x.RoleID == (int)UserRole.AircraftCrew  ).ToList();
            model.Destinations = _destService.GetAll().ToList();
            //model.Aircrafts =
            return View( model );
        }
        public ActionResult Delete()
        {
            var users = _userService.GetAll();
            CalendarViewModel model = new CalendarViewModel();
            model.Aircrafts = _actypeService.GetAll().ToList();
            model.Pilots = users.Where( x => x.RoleID == (int)UserRole.Pilot ).ToList();
            model.Crews = users.Where( x => x.RoleID == (int)UserRole.AircraftCrew ).ToList();
            model.Destinations = _destService.GetAll().ToList();
            //model.Aircrafts =
            return View( model );
        }

        [HttpPost]
        public ActionResult GetRegistrations(int aircraftid)
        {
            var registrations = _regService.GetAll().Where( x => x.AircraftID == aircraftid ).ToList();
            return Json( registrations, JsonRequestBehavior.AllowGet );
        }
        
        [HttpPost]
        public ActionResult Create(CalendarViewModel model)
        {
            model.LoggedUserID = Convert.ToInt32( User.Identity.GetUserId() );
            string msg = _calService.Create( model );
            return Json( new { Message = msg }, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult Update( CalendarViewModel model )
        {
            model.LoggedUserID = Convert.ToInt32( User.Identity.GetUserId() );
            string msg = _calService.Update( model );
            return Json( new { Message = msg }, JsonRequestBehavior.AllowGet );
        }
        [HttpPost]
        public ActionResult Remove( int id )
        {
            string msg = "";
            int loggedUserID = Convert.ToInt32( User.Identity.GetUserId() );
            _calService.DeleteCrewBySchedule( id, loggedUserID );
            msg= _calService.Delete( id, loggedUserID );
            return Json( new { Message = msg }, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult GetDetail(int id)
        {
            CalendarViewModel model = new CalendarViewModel();
            model= _calService.GetAll().Where( x => x.ID == id ).FirstOrDefault();
            return Json( model, JsonRequestBehavior.AllowGet );
        }


        [HttpPost]
        public JsonResult GetSchedule()
        {
            List<MyEvent> events = _calService.GetAllEvents();

            var json = new JavaScriptSerializer().Serialize( events ).Replace( @"\", "" ).Replace( @"/", "" );

            return Json( json, JsonRequestBehavior.AllowGet );
        }
        #endregion       
    }
}