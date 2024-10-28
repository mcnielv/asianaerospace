using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AAC.Framework.ICore;

namespace AAC.Core
{
    public class AACRepository : IRepository, IDisposable
    {
        private readonly IDBContext _context;
        public AACRepository( IDBContext context )
        {
            this._context = context;
        }

        #region IRepository
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            return _context.AsQueryable<T>();
        }
        public T Add<T>( T item ) where T : class
        {
            return _context.Add<T>( item );
        }
        public T Remove<T>( T item ) where T : class
        {
            return _context.Remove<T>( item );
        }
        public T Update<T>( T item ) where T : class
        {
            return _context.Update<T>( item );
        }
        public T Attach<T>( T item ) where T : class
        {
            return _context.Attach<T>( item );
        }
        public T Detach<T>( T item ) where T : class
        {
            return _context.Detach<T>( item );
        }
        //public int SaveChanges()
        //{
        //    return _context.SaveChanges();
        //}
        public void ExecuteTSQL( string tsql )
        {
            _context.ExecuteTSQL( tsql );
        }
        public T GetColumn<T>( string tsql )
        {
            return _context.GetColumn<T>( tsql );
        }
        public List<T> GetList<T>( string tsql ) where T : class
        {
            return _context.GetList<T>( tsql );
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            GC.SuppressFinalize( this );
        }
        #endregion
    }
}
