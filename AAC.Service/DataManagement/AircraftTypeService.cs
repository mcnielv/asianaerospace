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
using AAC.Web.Model.User;

namespace AAC.Service.DataManagement
{
    public class AircraftTypeService
    {
        private AAContext _context;
        #region Constructor
        public AircraftTypeService()
        {
            _context = new AAContext();
        }
        #endregion

        #region Method(s)
        public IQueryable<AircraftViewModel> GetAll()
        {
            var users = _context.AsQueryable<AircraftType>()
                    .Select( x => new AircraftViewModel()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description
                    } );

            return users;
        }
        public string Create( AircraftViewModel model )
        {
            string msg = string.Empty;
            try
            {
                var ctr = _context.AsQueryable<AircraftType>().Where( x => x.Name == model.Name ).Count();
                if ( ctr > 0 )
                    msg = "Aircraft already exists.";
                else
                {
                    AircraftType aircraft = new AircraftType()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };
                    _context.Add<AircraftType>( aircraft );
                    _context.SaveChanges();
                    msg = "Aircraft added.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Aircraft Service Error : Create()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
            return msg;
        }
        public string Update( AircraftViewModel model )
        {
            string msg = string.Empty;
            try
            {
                AircraftType aircraft = _context.AsQueryable<AircraftType>().Where( x => x.ID == model.ID ).FirstOrDefault();
                bool isExists = false;
                
                #region Check if new name changed
                if( aircraft.Name.Trim().ToUpper()!=model.Name.Trim().ToUpper())
                {
                    int ctr = _context.AsQueryable<AircraftType>().Where( x => x.Name.Trim() == model.Name.Trim() ).Count();
                    if ( ctr > 0 )
                        isExists = true;
                }
                #endregion

                if ( isExists )
                    msg = "Aircraft name already exists.";
                else
                {
                    if ( !string.IsNullOrEmpty( model.Name ) )
                        aircraft.Name = model.Name;
                    if ( !string.IsNullOrEmpty( model.Description ) )
                        aircraft.Description = model.Description;
                    aircraft.DateModified = DateTime.Now;

                    if ( aircraft != null )
                    {
                        _context.Update<AircraftType>( aircraft );
                        _context.SaveChanges();
                    }
                    msg = "Aircraft updated.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Aircraft Service Error : Update()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
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
                AircraftType aircraft = _context.AsQueryable<AircraftType>().Where( x => x.ID == id ).FirstOrDefault();
                if ( aircraft != null )
                {
                    _context.Remove<AircraftType>( aircraft );
                    _context.SaveChanges();
                    msg = "Aircraft deleted.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Aircraft Service Error : Delete()",
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
