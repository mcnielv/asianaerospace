using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Core;
using AAC.Data;
using AAC.Data.Entities;
using AAC.Web.Model;
using AAC.Web.Model.Destination;
using AAC.Web.Model.Enum;
using AAC.Web.Model.User;

using MNV.Infrastructure.Encryption;

namespace AAC.Service.Destination
{
    public class DestinationService
    {
        private AAContext _context;
        #region Constructor
        public DestinationService()
        {
            _context = new AAContext();
        }
        #endregion

        #region Method(s)
        public IQueryable<DestinationViewModel> GetAll()
        {
            var data = _context.AsQueryable<Data.Entities.Destination>()
                    .Select( x => new DestinationViewModel()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Description = x.Description
                    } );

            return data;
        }
        public string Create( DestinationViewModel model )
        {
            string msg = string.Empty;
            try
            {
                var ctr = _context.AsQueryable<Data.Entities.Destination>().Where( x => x.Name == model.Name ).Count();
                if ( ctr > 0 )
                    msg = "Destination already exists.";
                else
                {
                    Data.Entities.Destination destination = new Data.Entities.Destination()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };
                    _context.Add<Data.Entities.Destination>( destination );
                    _context.SaveChanges();
                    msg = "Destination added.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Destination Service Error : Create()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }
            return msg;
        }
        public string Update( DestinationViewModel model )
        {
            string msg = string.Empty;
            try
            {
                Data.Entities.Destination destination = _context.AsQueryable<Data.Entities.Destination>().Where( x => x.ID == model.ID ).FirstOrDefault();
                bool isExists = false;

                #region Check if new name changed
                if ( destination.Name.Trim().ToUpper() != model.Name.Trim().ToUpper() )
                {
                    int ctr = _context.AsQueryable<Data.Entities.Destination>().Where( x => x.Name.Trim() == model.Name.Trim() ).Count();
                    if ( ctr > 0 )
                        isExists = true;
                }
                #endregion

                if ( isExists )
                    msg = "Destination already exists.";
                else
                {
                    if ( !string.IsNullOrEmpty( model.Name ) )
                        destination.Name = model.Name;
                    if ( !string.IsNullOrEmpty( model.Description ) )
                        destination.Description = model.Description;
                    destination.DateModified = DateTime.Now;

                    if ( destination != null )
                    {
                        _context.Update<Data.Entities.Destination>( destination );
                        _context.SaveChanges();
                    }
                    msg = "Destination updated.";
                }
            }
            catch ( Exception ex )
            {
                msg = "Unexpected error encountered. Please contact your system administrator.";
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Destination Service Error : Update()",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                new LogsService().Create( error );
            }
            return msg;
        }
        public string Delete( int id )
        {
            //NOTE: Original Logic is to delete, but we change it to just deactivate the user.
            string msg = string.Empty;
            try
            {
                Data.Entities.Destination destination = _context.AsQueryable<Data.Entities.Destination>().Where( x => x.ID == id ).FirstOrDefault();
                if ( destination != null )
                {
                    _context.Remove<Data.Entities.Destination>( destination );
                    _context.SaveChanges();
                    msg = "Destination deleted.";
                }
            }
            catch ( Exception ex )
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Destination Service Error : Delete()",
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
