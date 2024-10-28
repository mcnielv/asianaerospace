using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Interfaces
{
    public interface IQueryObject
    {
        int Execute(IDbContext context);
    }

    public interface IQueryObject<out T>
    {
        IEnumerable<T> Execute(IDbContext context);
    }
}
