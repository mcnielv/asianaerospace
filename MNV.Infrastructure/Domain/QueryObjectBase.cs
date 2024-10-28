using AdoptAClassroom.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoptAClassroom.Infrastructure.Domain
{
    public abstract class QueryObjectBase<T> : IQueryObject<T>
    {
        protected Func<IDbContext, IQueryable<T>> ContextQuery { get; set; }

        protected IDbContext Context { get; set; }

        protected void CheckContextAndQuery()
        {
            if (this.Context == null)
                throw new InvalidOperationException("Context cannot be null.");

            if (this.ContextQuery == null)
                throw new InvalidOperationException("Null Query cannot be executed.");
        }
        
        protected virtual IQueryable<T> ExtendQuery()
        {
            return this.ContextQuery(Context);
        }

        public IEnumerable<T> Execute(IDbContext context)
        {
            this.Context = context;
            CheckContextAndQuery();
            var query = this.ExtendQuery();
            return query.AsEnumerable() ?? Enumerable.Empty<T>();
        }
    }
}
