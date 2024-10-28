using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Interfaces
{
    public interface IRepository
    {
        T Find<T>(IScalarObject<T> query);
        IEnumerable<T> Find<T>(IQueryObject<T> query);
        T Add<T>(T item) where T : class;
        T Remove<T>(T item) where T : class;
        T Update<T>(T item) where T : class;
        T Attach<T>(T item) where T : class;
        T Detach<T>(T item) where T : class;
        int SaveChanges();
    }
}
