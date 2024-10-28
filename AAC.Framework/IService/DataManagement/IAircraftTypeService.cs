using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Web.Model.Aircraft;

namespace AAC.Framework.IService.DataManagement
{
    public interface IAircraftTypeService
    {
        IQueryable<AircraftViewModel> GetAll();
        string Create( AircraftViewModel model );
        string Update( AircraftViewModel model );
        string Delete( int id );    
    }
}
