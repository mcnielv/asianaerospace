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

namespace AAC.Service
{
    public class LogsService
    {
        private AAContext _context;

        #region Constructor
        public LogsService()
        {
            _context = new AAContext();
        }
        #endregion

        #region Method(s)
        public IQueryable<LogsViewModel> GetAll()
        {
            var logs = _context.AsQueryable<Data.Entities.Logs>()
                .Select( x => new LogsViewModel()
                {
                    ID = x.ID,
                    ActionType = x.ActionType,
                    Description = x.Description,
                    ModifiedBy = x.ModifiedBy,
                    DateModified = x.DateModified.ToString()
                } );

            foreach (var log in logs)
            {
                log.DateModified = DateTime.Parse( log.DateModified ).ToString( "MM/dd/yyyy HH:mm" );
            }

            return logs;

        }
        public string Create(LogsViewModel model)
        {
            string msg = string.Empty;
            try
            {
                Logs log = new Logs()
                {
                    ActionType = model.ActionType,
                    Description = model.Description,
                    DateModified = DateTime.Parse( model.DateModified ),
                    ModifiedBy = model.ModifiedBy
                };
                _context.Add<Logs>( log );
                _context.SaveChanges();
                msg = "Log Created.";
            }
            catch (Exception ex)
            {
                LogsViewModel error = new LogsViewModel()
                {
                    ActionType = ActionType.Error.ToString(),
                    Description = ex.Message + "\n" + ex.StackTrace,
                    ModifiedBy = "Log Service Error",
                    DateModified = DateTime.Now.ToString( "MM/dd/yyyy HH:mm" )
                };
                this.Create( error );
                msg = "Unexpected error encountered. Please contact your system administrator.";
            }

            return msg;
        }
        #endregion
    }
}
