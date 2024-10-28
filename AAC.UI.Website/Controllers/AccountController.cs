using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AAC.UI.Website.Models;
using LoginViewModel = AAC.Web.Model.Account.LoginModel;
using AAC.Web.Model.Enum;

namespace AAC.UI.Website.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        private async Task SignInAsync( ApplicationUser user, bool isPersistent )
        {
            AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie );
            AuthenticationManager.SignIn( new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync( UserManager ) );
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie );
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            LoginViewModel logVM = new LoginViewModel();
            //var result = await SignInManager.PasswordSignInAsync( model.Username, model.Password, true, false, out logVM  );
            string password = string.Empty;
            if ( !string.IsNullOrEmpty( model.Password ) )
                password = MNV.Infrastructure.Encryption.ThreeDES.Encrypt( model.Password );

            var user = await UserManager.FindAsync( model.Username ?? string.Empty, password );
            if ( user != null )
            {
                if ( user.Active == true )
                {
                    await SignInAsync( user, false );
                    return RedirectToAction( "Home", "Profile" );
                }
                else
                    return RedirectToAction( "Login", "Account", new { msg = "Unable to login inactive user." } );
            }
            return RedirectToAction( "Login", "Account", new { msg = "Invalid username or password." } );
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        string userid = Guid.NewGuid().ToString();
            //        var user = new ApplicationUser
            //        {
            //            UserName = logVM.Username,
            //            Id = logVM.UserID,
            //            Password = model.Password,
            //            Email = logVM.Email,
            //            FullName = logVM.FullName,
            //            RoleID = logVM.RoleID,                        
            //            UserId = logVM.UserID
            //        };

            //        //var result2 = await UserManager.CreateAsync( user, model.Password );
            //        //UserManager.AddToRole( user.Id, logVM.RoleName );
            //        //if ( result2.Succeeded )
            //        //{
            //            await SignInAsync(user,false);// SignInManager.SignInAsync( user, isPersistent: false, rememberBrowser: false );
            //            switch ( (UserRole)logVM.RoleID )
            //            {
            //                case UserRole.Administrator:
            //                    return RedirectToAction( "Home", "Pilot" );
            //                    //RedirectToLocal( "" );
            //                case UserRole.Pilot:
            //                    return RedirectToAction( "Home", "Pilot" );
            //            }
            //            return RedirectToLocal( returnUrl );
            //        //}

            //        return RedirectToLocal( returnUrl );
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = true });
            //    case SignInStatus.Failure:
            //    default:
            //        ModelState.AddModelError("", "Invalid login attempt.");
            //        return View(model);
            //}
        }
        
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                  
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Login", "Home", new { returnUrl="" } );
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

       
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}