using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Web.Model.Destination;

namespace AAC.Framework.IService.Destination
{
    public interface IDestinationService
    {
        IQueryable<DestinationViewModel> GetAll();
        string Create( DestinationViewModel model );
        void Update( DestinationViewModel model );
        void Delete( int id );
    }
}
