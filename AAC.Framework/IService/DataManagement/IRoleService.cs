using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Web.Model.Role;

namespace AAC.Framework.IService.DataManagement
{
    public interface IRoleService
    {
        IQueryable<RoleViewModel> GetAllRoles();
    }
}
