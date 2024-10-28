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

using AAC.Web.Model.Enum;

namespace AAC.UI.Website.Controllers
{
    public class CommonController : Controller
    {
        private ApplicationUserManager _userManager;
        public CommonController( )
        {
        }
        public CommonController( ApplicationUserManager userManager )
        {
            _userManager = userManager;
        }
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
        public PartialViewResult _WelcomeText()
        {
            var user = UserManager.FindById( Convert.ToInt32( User.Identity.GetUserId() ) );
            ViewBag.FullName = user.LastName + ", " + user.FirstName;
            return PartialView();
        }
       
        public PartialViewResult _Navigations()
        {
            var user = UserManager.FindById( Convert.ToInt32( User.Identity.GetUserId() ) );
            ViewBag.RoleID = user.RoleID;
            //string roleid = user.RoleID.ToString();
            return PartialView();
        }

        #region DivLoading
        public PartialViewResult _DivLoading()
        {
            return PartialView();
        }
        #endregion
    }
}