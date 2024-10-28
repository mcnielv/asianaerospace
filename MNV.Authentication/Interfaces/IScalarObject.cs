using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Interfaces
{
    public interface IScalarObject<out T>
    {
        T Execute(IDbContext context);
    }
}
