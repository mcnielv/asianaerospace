using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Interfaces
{
    public interface IOrderFee
    {        
        decimal HandlingFee { get; }
        decimal MinShipping { get; }
        decimal FlatShipping { get; }
        decimal NoShippingFrom { get; }        
    }
}
