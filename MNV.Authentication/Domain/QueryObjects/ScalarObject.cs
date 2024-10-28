using MNV.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Domain.QueryObjects
{
    public class ScalarObject<T> : IScalarObject<T>
    {
        public Func<IDbContext, T> ContextQuery { get; set; }

        public void CheckContextAndQuery(IDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            if (this.ContextQuery == null)
                throw new InvalidOperationException("Null Query cannot be executed.");
        }

        #region IScalarObject<T> Members

        public T Execute(IDbContext context)
        {
            CheckContextAndQuery(context);
            return this.ContextQuery(context);
        }
        
        #endregion
    }
}
