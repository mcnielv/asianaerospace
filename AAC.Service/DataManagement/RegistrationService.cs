using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Core;
using AAC.Data;
using AAC.Data.Entities;
using AAC.Web.Model;
using AAC.Web.Model.Aircraft;
using AAC.Web.Model.Enum;
using AAC.Web.Model.Registration;
using AAC.Web.Model.User;

namespace AAC.Service.DataManagement
{
    public class RegistrationService
    {
        private AAContext _context;
        #region Constructor
        public RegistrationService()
        {
            _context = new AAContext();
        }
        #endregion

        #region Method(s)
        public IQueryable<RegistrationViewModel> GetAll()
        {
            var registrations = _context.AsQueryable<Registration>()
                    .Select( x => new RegistrationViewModel()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        AircraftID = x.AircraftID,
                        AircraftName = x.AircraftType.Name,
                        Description = x.Description
                    } );

            return registrations;
        }
        public string Create( RegistrationViewModel model )
        {
            string msg = string.Empty;
            try
            {
                var ctr = _context.AsQueryable<Registration>().Where( x => x.Name == model.Name ).Count();

                if ( ctr > 0 )
                    msg = "Aircraft registration already exists.";
                else
                {
                    Registration registration = new Registration()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        AircraftID = model.AircraftID,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };
                    _context.Add<Registration>( registration );
                    _context.SaveChanges();
                    msg = "Aircraft registration added.";
                }
            }
            catch ( Exception ex )
            {
                //TODO: Log Error here.
                msg = "Unexpected error encountered. Please contact your system administrator.";
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Registration Service Error : Create()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
            }
            return msg;
        }
        public string Update( RegistrationViewModel model )
        {
            string msg = string.Empty;
            try
            {
                Registration registration = _context.AsQueryable<Registration>().Where( x => x.ID == model.ID ).FirstOrDefault();
                bool isExists = false;

                #region Check if new name changed
                if ( registration.Name.Trim().ToUpper() != model.Name.Trim().ToUpper() )
                {
                    int ctr = _context.AsQueryable<AircraftType>().Where( x => x.Name.Trim() == model.Name.Trim() ).Count();
                    if ( ctr > 0 )
                        isExists = true;
                }
                #endregion

                if ( isExists )
                    msg = "Aircraft registration already exists.";
                else
                {
                    if ( !string.IsNullOrEmpty( model.Name ) )
                        registration.Name = model.Name;
                    if ( !string.IsNullOrEmpty( model.Description ) )
                        registration.Description = model.Description;
                    if ( model.AircraftID > 0 )
                        registration.AircraftID = model.AircraftID;

                    registration.DateModified = DateTime.Now;

                    if ( registration != null )
                    {
                        _context.Update<Registration>( registration );
                        _context.SaveChanges();
                    }
                    msg = "Aircraft registration updated.";
                }
            }
            catch ( Exception ex )
            {
                msg = "Unexpected error encountered. Please contact your system administrator.";
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Registration Service Error : Update()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                //log error
            }
            return msg;
        }
        public string Delete( int id )
        {
            //NOTE: Original Logic is to delete, but we change it to just deactivate the user.
            string msg = string.Empty;
            try
            {
                Registration registration = _context.AsQueryable<Registration>().Where( x => x.ID == id ).FirstOrDefault();
                if ( registration != null )
                {
                    _context.Remove<Registration>( registration );
                    _context.SaveChanges();
                    msg = "Aircraft registration deleted.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Registration Service Error : Delete()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
            return msg;
        }
        #endregion
    }
}
