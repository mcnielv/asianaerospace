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

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using AAC.Web.Model.Enum;

using AAC.Web.Model.User;
using AAC.Web.Model.Role;
using AAC.Service.DataManagement;

namespace AAC.UI.Website.Controllers.DataManagement
{
    [Authorize]
    public class UserManagementController : Controller
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
        private UserService _userService;
        private RoleService _roleService;

        #region Constructor
        public UserManagementController()
        {
            _userService = new UserService();
            _roleService = new RoleService();
        }
        public UserManagementController( ApplicationUserManager userManager )
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

        #region Action(s)
        // GET: User
        public ActionResult Index()
        {
            UserViewModel model = new UserViewModel();
            List<RoleViewModel> roles = new List<RoleViewModel>();

            roles = _roleService.GetAllRoles().Where( x => x.Active == true ).ToList();
            model.RoleViewModels = roles;

            return View(model);
        }

        [HttpPost]
        public ActionResult LoadUserDetails(int id )
        {
            var model = _userService.GetAll().Where( x => x.ID == id ).FirstOrDefault();
            return Json( model, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult CreateNewUser(UserViewModel model)
        {
            string msg = _userService.Create( model );
            return Json( new { Message = msg }, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult DeleteUser( int id )
        {
            string msg = _userService.Delete( id );
            return Json( new { Message = msg }, JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult ResetPassword( int id )
        {
            string msg = _userService.ResetPassword( id );
            return Json( new { Message = msg }, JsonRequestBehavior.AllowGet );
        }
        #endregion

        #region JQGRID
        #region GridView
        [HttpPost]
        public ActionResult LoadjqData( string sidx, string sord, int page, int rows,
                bool _search, string searchField, string searchOper, string searchString )
        {
            // Get the list of users
            IQueryable<UserViewModel> users = _userService.GetAll();

            // If search, filter the list against the search condition.
            // Only "contains" search is implemented here.
            var filteredRoles = users;
            if ( _search )
            {
                switch ( searchOper )
                {
                    case "eq":
                        filteredRoles = users.Where( x => x.Username == searchString ).AsQueryable();
                        break;
                        //case "cn":
                        //    filteredRoles = category.Where( s => s.Name.Contains( searchString ) ).AsQueryable();
                        //    break;
                }

            }
            // Sort the student list
            var sortedRoles = SortIQueryable<UserViewModel>( filteredRoles, sidx, sord ).AsEnumerable();

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
                        s.RoleName,
                        s.Username,
                        s.FirstName,
                        s.MiddleName,
                        s.LastName,
                        s.Status
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