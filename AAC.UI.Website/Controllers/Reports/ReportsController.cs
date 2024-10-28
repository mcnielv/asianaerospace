using System;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Security.Principal;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using AAC.Web.Model.Enum;
using AAC.Service;
using AAC.Service.Reports;
using AAC.Web.Model;
//using AAC.Web.Model.Aircraft;

using MNV.Infrastructure.Converter;
using AAC.Web.Model.Reports;

namespace AAC.UI.Website.Controllers.Reports
{
    [Authorize]
    public class ReportsController : Controller
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

        private ReportService _rptService;
        private LogsService _logService;

        #region Constructor
        public ReportsController()
        {
            _rptService = new ReportService();
            _logService = new LogsService();
        }
        public ReportsController( ApplicationUserManager userManager )
        {
            _rptService = new ReportService();
            _logService = new LogsService();
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

        #region Action(s)
        public ActionResult Schedule()
        {
            return View();
        }

        public ActionResult Logs()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CsvConvert( string start, string end )
        {
            var model = _rptService.GetAllSchedules( start, end );

            string filename = _rptService.GenerateCSV( model, Server.MapPath( "~/pdffile/RptSchedule.csv" ) );
            return Json( new { Filename = filename }, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult GetAllSchedules( string start, string end )
        {
            var model = _rptService.GetAllSchedules( start, end );
           
            return Json( model, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult ConvertToPdf( string html )
        {
            var model = "";
            if ( !string.IsNullOrEmpty( html ) )
            {
                HtmlToPdf htmltopdf = new HtmlToPdf();
                htmltopdf.Convert( HttpUtility.UrlDecode( html ), Server.MapPath( "~/pdffile/" ), "RptSchedule" );
            }
            return Json( model, JsonRequestBehavior.AllowGet );
        }
        #endregion

        #region JQGRID
        #region GridView
        [HttpPost]
        public ActionResult LoadLogs( string sidx, string sord, int page, int rows,
                bool _search, string searchField, string searchOper, string searchString )
        {
            // Get the list of users
            IQueryable<LogsViewModel> logs = _logService.GetAll().OrderByDescending( x => x.DateModified );

            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredRoles = logs;
            if ( _search )
            {
                switch ( searchOper )
                {
                    case "eq":
                        filteredRoles = logs.Where( x => x.ModifiedBy == searchString ).AsQueryable();
                        break;
                        //case "cn":
                        //    filteredRoles = category.Where( s => s.Name.Contains( searchString ) ).AsQueryable();
                        //    break;
                }

            }
            // Sort the student list
            var sortedRoles = SortIQueryable<LogsViewModel>( filteredRoles, sidx, sord ).AsEnumerable();

            // Calculate the total number of pages
            var totalRecords = filteredRoles.Count();
            var totalPages = (int)Math.Ceiling( (double)totalRecords / (double)rows );

            // NOTE:XXXX Prepare the data to fit the requirement of jQGrid
            var data =
                sortedRoles.Select( s => new
                {
                    id = s.ID,
                    cell = new object[] {
                        s.ID,
                        s.ActionType,
                        s.Description,
                        s.ModifiedBy,
                        s.DateModified
                    }
                } ).ToArray();

            // Send the data to the jQGrid
            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = data.Skip( ( page - 1 ) * rows ).Take( rows )
            };

            return Json( jsonData, JsonRequestBehavior.AllowGet );
        }
        #endregion
        #region SORT QUERY
        // Utility method to sort IQueryable given a field name as "string"
        // May consider to put in a cental place to be shared
        private IQueryable<T> SortIQueryable<T>( IQueryable<T> data, string fieldName, string sortOrder )
        {
            if ( string.IsNullOrWhiteSpace( fieldName ) )
                return data;
            if ( string.IsNullOrWhiteSpace( sortOrder ) )
                return data;

            var param = Expression.Parameter( typeof( T ), "i" );
            Expression conversion = Expression.Property( param, fieldName );
            var mySortExpression = Expression.Lambda<Func<T, object>>( conversion, param );

            return ( sortOrder == "desc" ) ? data.OrderByDescending( mySortExpression )
                : data.OrderBy( mySortExpression );
        }
        #endregion
        #endregion

    }
}