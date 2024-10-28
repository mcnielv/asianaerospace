using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Web.Model.User;

namespace AAC.Framework.IService.DataManagement
{
    public interface IUserService
    {
        IQueryable<UserViewModel> GetAll();
        string Create( UserViewModel model );
        string Update( UserViewModel model );
        string ResetPassword( int id );
        string Delete( int id );
    }
}
