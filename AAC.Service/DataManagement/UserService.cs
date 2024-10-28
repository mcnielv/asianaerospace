using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Core;
using AAC.Data;
using AAC.Data.Entities;
using AAC.Web.Model;
using AAC.Web.Model.Enum;
using AAC.Web.Model.User;

using MNV.Infrastructure.Encryption;

namespace AAC.Service.DataManagement
{
    public class UserService
    {
        private AAContext _context;
        #region Constructor
        public UserService()
        {
            _context = new AAContext();
        }
        #endregion

        #region Method(s)
        public IQueryable<UserViewModel> GetAll()
        {
            var users = _context.AsQueryable<User>()
                    .Select( x => new UserViewModel()
                    {
                        ID = x.ID,
                        RoleID = x.RoleID,
                        RoleName = x.Role.Name,
                        Username = x.Username,
                        Password = x.Password,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        MiddleName = x.MiddleName,
                        Address = x.Address,
                        Phone = x.Phone,
                        Email = x.Email,
                        SSS = x.SSS,
                        TIN = x.TIN,
                        Status = x.Active == true ? "Active" : "Inactive"
                    } );

            return users;
        }
        public string Create(UserViewModel model)
        {
            string msg = string.Empty;
            try
            {
                var ctr = _context.AsQueryable<User>().Where( x => x.Username == model.Username || x.Email == model.Email ).Count();
                if ( ctr > 0 )
                    msg = "Your profile email or username already exists.";
                else
                {
                    User user = new User()
                    {
                        RoleID = model.RoleID,
                        Username = model.Username,
                        Password = ThreeDES.Encrypt("Password1"),//DEFAULT PASSWORD: Password1
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        MiddleName = model.MiddleName,
                        Email = model.Email,
                        Address = model.Address ?? "",
                        Phone = model.Phone ?? "",
                        SSS = model.SSS ?? "",
                        TIN = model.TIN ?? "",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Active = true
                    };
                    _context.Add<User>( user );
                    _context.SaveChanges();
                    msg = "New profile added.";
                }
            }
            catch(Exception ex)
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "User Service Error : Create()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
            return msg;
        }
        public string Update(UserViewModel model)
        {
            string msg = string.Empty;
            try
            {
                User user = _context.AsQueryable<User>().Where( x => x.ID == model.ID ).FirstOrDefault();
                bool isExists = false;
                #region  check if current username and email is not equal to new value
                isExists = this.IsExists( user.ID, user.Username, model.Username, user.Email, model.Email );
                #endregion

                if ( isExists )
                    msg = "Your profile email or username already exists.";
                else
                {
                    if ( !string.IsNullOrEmpty( model.Password ) )
                        user.Password = ThreeDES.Encrypt( model.Password );
                    if ( !string.IsNullOrEmpty( model.Username ) )
                        user.Username = model.Username;
                    if ( !string.IsNullOrEmpty( model.FirstName ) )
                        user.FirstName = model.FirstName;
                    if ( !string.IsNullOrEmpty( model.LastName ) )
                        user.LastName = model.LastName;
                    if ( !string.IsNullOrEmpty( model.MiddleName ) )
                        user.MiddleName = model.MiddleName;
                    if ( !string.IsNullOrEmpty( model.Address ) )
                        user.Address = model.Address;
                    if ( !string.IsNullOrEmpty( model.Phone ) )
                        user.Phone = model.Phone;
                    if ( !string.IsNullOrEmpty( model.Email ) )
                        user.Email = model.Email;
                    if ( !string.IsNullOrEmpty( model.SSS ) )
                        user.SSS = model.SSS;
                    if ( !string.IsNullOrEmpty( model.TIN ) )
                        user.TIN = model.TIN;
                    if ( model.RoleID != 0 )
                        user.RoleID = model.RoleID;
                    user.DateModified = DateTime.Now;

                    if ( user != null )
                    {
                        _context.Update<User>( user );
                        _context.SaveChanges();
                    }
                    msg = "Profile updated.";
                }
            }
            catch ( Exception ex )
            {
                msg = "Unexpected error encountered. Please contact your system administrator.";
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "User Service Error : Update()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
            }
            return msg;
        }
        public string ResetPassword(int id)
        {
            string msg = string.Empty;
            var user = _context.AsQueryable<User>().Where( x => x.ID == id ).FirstOrDefault();
            if ( user != null )
            {
                try
                {
                    user.Password = ThreeDES.Encrypt( "Password1" );//DEFAULT PASSWORD: Password1
                    _context.Update<User>( user );
                    _context.SaveChanges();
                }
                catch( Exception ex )
                {
                    LogsViewModel error = new LogsViewModel()
                    {
                        ActionType = ActionType.Error.ToString(),
                        Description = ex.Message + "\n" + ex.StackTrace,
                        ModifiedBy = "User Service Error : ResetPassword()",
                        DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                    };
                    new LogsService().Create( error );
                    msg = "Unexpected error encountered. Please contact your system administrator.";
                }
            }

            return msg;
        }
        public string Delete(int id)
        {
            //NOTE: Original Logic is to delete, but we change it to just deactivate the user.
            string msg = string.Empty;
            try
            {
                User user = _context.AsQueryable<User>().Where( x => x.ID == id ).FirstOrDefault();
                if(user != null)
                {
                    user.Active = false;
                    _context.Update<User>( user );
                    _context.SaveChanges();
                    msg = "Profile deactiveted.";
                }
                //if ( user == null )
                //    msg = "Unable to delete profile, Please contact your system administrator.";
                //else
                //{
                //    _context.Remove<User>( user );
                //    _context.SaveChanges();
                //    msg = "Profile deleted.";
                //}
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "User Service Error : Delete()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";                
            }
            return msg;
        }
        #endregion

        #region Private Method(s)
        private bool IsExists(int id, string origUsername,string newUsername, string origEmail, string newEmail)
        {
            bool ret = false;
            if( !string.Equals(origUsername,newUsername))
            {
                //check if new username exists
                int unameCount = _context.AsQueryable<User>().Where( x => x.Username == newUsername ).Count();
                if ( unameCount > 0 )
                    ret = true;
            }
            if ( !string.Equals( origEmail, newEmail ) )
            {
                //check if new username exists
                int emailCount = _context.AsQueryable<User>().Where( x => x.Username == newEmail ).Count();
                if ( emailCount > 0 )
                    ret = true;
            }
            return ret;
        }
        #endregion
    }
}
