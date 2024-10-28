using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Web.Model.Registration;

namespace AAC.Framework.IService.DataManagement
{
    public interface IRegistrationService
    {
        IQueryable<RegistrationViewModel> GetAll();
        string Create( RegistrationViewModel model );
        string Update( RegistrationViewModel model );
        string Delete( int id );
    }
}
