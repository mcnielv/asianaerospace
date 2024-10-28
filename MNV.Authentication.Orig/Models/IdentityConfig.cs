using MNV.Authentication.Interfaces;
using MNV.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Authentication.Model
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            string loginUrl = AppSettings.Setting<string>("loginUrl");
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);            

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {                
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });            
        }
    }
}
