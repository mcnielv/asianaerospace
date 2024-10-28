using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.ICore
{
    public interface IRepository
    {
        IQueryable<T> AsQueryable<T>() where T : class;
        T Add<T>( T item ) where T : class;
        T Remove<T>( T item ) where T : class;
        T Update<T>( T item ) where T : class;
        T Attach<T>( T item ) where T : class;
        T Detach<T>( T item ) where T : class;
        //int SaveChanges();
        void ExecuteTSQL( string tsql );
        T GetColumn<T>( string tsql );
        List<T> GetList<T>( string tsql ) where T : class;
    }
}
