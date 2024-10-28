using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Core;
using AAC.Data;
using AAC.Data.Entities;
using AAC.Web.Model.Account;

namespace AAC.Service.Account
{
    public class LoginService
    {
        public LoginService() { }

        #region Methods
        public LoginModel LetMeIn( LoginModel model )
        {
            string message = "Invalid username/password";
            model.UserID = 0;
            AAContext context = new AAContext();
            User user = context.AsQueryable<User>()
                    .Where( x => x.Username == model.Username.Trim() && x.Password == model.Password.Trim() )
                    .FirstOrDefault();
            if ( user != null )
                if ( user.ID > 0 )
                {
                    message = "";
                    model.UserID = user.ID;
                    model.RoleID = user.RoleID;
                    model.Username = user.Username;
                    model.FullName = string.Format( "{0}, {1} {2}", user.LastName, user.FirstName, user.MiddleName );
                    model.Email = user.Email;
                    model.RoleName = user.Role.Name;
                }
            model.Message = message;
            return model;
        }
        
        #endregion
    }
}
