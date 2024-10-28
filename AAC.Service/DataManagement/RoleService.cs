using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Core;
using AAC.Data;
using AAC.Data.Entities;
using AAC.Web.Model.Role;

namespace AAC.Service.DataManagement
{
    public class RoleService
    {
        private AAContext _context;
        #region Constructor
        public RoleService() { _context = new AAContext(); }
        #endregion

        #region Method(s)
        public IQueryable<RoleViewModel> GetAllRoles()
        {
            var roles = _context.AsQueryable<Role>()
                 .Select( x => new RoleViewModel()
                 {
                     ID = x.ID,
                     Name = x.Name,
                     Description = x.Description,
                     Active = x.Active
                 } );

            return roles;
        }
        #endregion
    }
}
