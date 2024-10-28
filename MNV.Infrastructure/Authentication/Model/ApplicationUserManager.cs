using MNV.Infrastructure.Authentication.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Authentication.Model
{
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store) { }

        public static ApplicationUserManager Create()
        {
            var manager = new ApplicationUserManager(new ApplicationUserStore());
            return manager;
        }
    }
}
