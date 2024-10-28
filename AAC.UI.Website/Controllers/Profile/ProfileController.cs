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
using AAC.Web.Model.User;
using AAC.Web.Model.Enum;
namespace AAC.UI.Website.Controllers.Profile
{
    [Authorize]
    public class ProfileController : Controller
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
        #region Constructor
        public ProfileController()
        {
            _userService = new UserService();
        }
        public ProfileController( ApplicationUserManager userManager )
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

        #region Controller Action(s)
        public ActionResult Home()
        {
            int id = Convert.ToInt32( User.Identity.GetUserId() );
            var model = _userService.GetAll().Where( x => x.ID == id ).FirstOrDefault();
            string decryptedpw = MNV.Infrastructure.Encryption.ThreeDES.Decrypt( model.Password );
            model.Password = decryptedpw;
            return View(model);
        }
        [HttpPost]
        public ActionResult Update( UserViewModel model )
        {
            //update data
            string msg = _userService.Update( model );
            return Json( new { Message = msg }, JsonRequestBehavior.AllowGet );
        }
        #endregion
    }
}