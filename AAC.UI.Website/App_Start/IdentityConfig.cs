using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using AAC.UI.Website.Models;
using AAC.Web.Model.Account;

namespace AAC.UI.Website
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    //// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    //public class ApplicationUserManager : UserManager<ApplicationUser>
    //{
    //    public ApplicationUserManager(IUserStore<ApplicationUser> store)
    //        : base(store)
    //    {
    //    }

    //    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
    //    {
    //        var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
    //        // Configure validation logic for usernames
    //        manager.UserValidator = new UserValidator<ApplicationUser>(manager)
    //        {
    //            AllowOnlyAlphanumericUserNames = false,
    //            RequireUniqueEmail = true
    //        };

    //        // Configure validation logic for passwords
    //        manager.PasswordValidator = new PasswordValidator
    //        {
    //            RequiredLength = 6,
    //            RequireNonLetterOrDigit = true,
    //            RequireDigit = true,
    //            RequireLowercase = true,
    //            RequireUppercase = true,
    //        };

    //        // Configure user lockout defaults
    //        manager.UserLockoutEnabledByDefault = true;
    //        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //        manager.MaxFailedAccessAttemptsBeforeLockout = 5;

    //        // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
    //        // You can write your own provider and plug it in here.
    //        manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
    //        {
    //            MessageFormat = "Your security code is {0}"
    //        });
    //        manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
    //        {
    //            Subject = "Security Code",
    //            BodyFormat = "Your security code is {0}"
    //        });
    //        manager.EmailService = new EmailService();
    //        manager.SmsService = new SmsService();
    //        var dataProtectionProvider = options.DataProtectionProvider;
    //        if (dataProtectionProvider != null)
    //        {
    //            manager.UserTokenProvider = 
    //                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
    //        }
    //        return manager;
    //    }
    //}

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager( IUserStore<ApplicationUser, int> store )
            : base( store ) { }

        public static ApplicationUserManager Create()
        {
            var manager = new ApplicationUserManager( new ApplicationUserStore() );
            return manager;
        }
    }
    //public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    //{
    //    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
    //        : base(userManager, authenticationManager)
    //    {
    //    }

    //    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    //    {
    //        return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    //    }

    //    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    //    {
    //        return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    //    }
    //    public override Task<SignInStatus> PasswordSignInAsync( string userName, string password, bool isPersistent, bool shouldLockout )
    //    {
    //        AAC.Web.Model.Account.LoginModel model = new Service.Account.LoginService()
    //                        .LetMeIn( new AAC.Web.Model.Account.LoginModel() { Username = userName, Password = password } );
    //        var user = new ApplicationUser
    //        {
    //            UserName = model.Username,
    //            Id = model.UserID.ToString(),
    //            PasswordHash = model.Password
    //            //,FullName = model.FullName,
    //            //RoleID = model.RoleID
    //        };
    //        //var result = await UserManager.CreateAsync( user, model.Password );
    //        //UserManager.CreateAsync( user, model.Password );
    //        //SignInManager.SignInAsync( user, isPersistent: false, rememberBrowser: false );
    //        return Task.FromResult( SignInStatus.Failure );
    //    }
    //    public Task<SignInStatus> PasswordSignInAsync( string userName, string password, bool isPersistent, bool shouldLockout, out LoginModel model)
    //    {
    //        model = new Service.Account.LoginService()
    //                         .LetMeIn( new AAC.Web.Model.Account.LoginModel() { Username = userName, Password = password } );
    //      if( model.UserID > 0 )
    //            return Task.FromResult( SignInStatus.Success );
    //      else
    //        return Task.FromResult( SignInStatus.Failure );
    //    }
    //    //public override Task SignInAsync( ApplicationUser user, bool isPersistent, bool rememberBrowser )
    //    //{
    //    //    AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie );
    //    //    ClaimsIdentity claimsIdentity = new ClaimsIdentity( DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role );
    //    //    claimsIdentity.AddClaim( new Claim( "UserId", user.Id ) );
    //    //    claimsIdentity.AddClaim( new Claim( "RoleID", user.RoleID.ToString() ) );
    //    //    AuthenticationManager.SignIn( new AuthenticationProperties() { IsPersistent = isPersistent }, claimsIdentity );
    //    //    return Task.FromResult( SignInStatus.Success );
    //    //}
    //    //private async Task SignInAsync( ApplicationUser user, bool isPersistent )
    //    //{
    //    //    AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie );
    //    //    ClaimsIdentity claimsIdentity = new ClaimsIdentity( DefaultAuthenticationTypes.ApplicationCookie, ClaimTypes.NameIdentifier, ClaimTypes.Role );
    //    //    claimsIdentity.AddClaim( new Claim( ClaimTypes.NameIdentifier, "MyCustomID", "http://www.w3.org/2001/XMLSchema#string" ) );
    //    //    claimsIdentity.AddClaim( new Claim( ClaimTypes.Name, "MyCustomUser", "http://www.w3.org/2001/XMLSchema#string" ) );
    //    //    claimsIdentity.AddClaim( new Claim( "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Custom Identity", "http://www.w3.org/2001/XMLSchema#string" ) );
    //    //    AuthenticationManager.SignIn( new AuthenticationProperties() { IsPersistent = isPersistent }, claimsIdentity );
    //    //}
    //}
    public class ApplicationSignInManager : SignInManager<ApplicationUser, int>
    {
        public ApplicationSignInManager
            ( ApplicationUserManager userManager, IAuthenticationManager authenticationManager )
            : base( userManager, authenticationManager ) { }

        public static ApplicationSignInManager Create
            ( IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context )
        {
            return new ApplicationSignInManager
                ( context.GetUserManager<ApplicationUserManager>(), context.Authentication );
        }
    }
}
