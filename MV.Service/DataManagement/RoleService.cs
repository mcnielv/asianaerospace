using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Framework.ICore;
using AAC.Framework.IService.DataManagement;
using AAC.Web.Model.Role;
using AAC.Data.Entities;

namespace MV.Service.DataManagement
{
    public class RoleService : IRoleService
    {
        private readonly IRepository _repository;

        #region Constructor
        public RoleService( IRepository repository )
        {
            this._repository = repository;
        }
        #endregion

        #region Method(s)
        public IQueryable<RoleViewModel> GetAllRoles()
        {
            IQueryable<RoleViewModel> roles = null;
            roles = _repository.AsQueryable<Role>()
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
