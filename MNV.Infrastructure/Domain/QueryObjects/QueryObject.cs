using MNV.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Domain.QueryObjects
{
    public class QueryObject //: IQueryObject
    {
        public Func<IDbContext, int> ContextQuery { get; set; }

        protected void CheckContextAndQuery(IDbContext context)
        {
            if (context == null)
                throw new ArgumentException("context");
            
            if (this.ContextQuery == null)
                throw new InvalidOperationException("Null Query cannot be executed.");
        }

        public int Execute(IDbContext context)
        {
            CheckContextAndQuery(context);
            return this.ContextQuery(context);
        }
    }

    public class QueryObject<T> : QueryObjectBase<T>
    {
        protected override IQueryable<T> ExtendQuery()
        {
            var source = base.ExtendQuery();
            source = this.AppendExpressions(source);
            return source;
        }

        public IQueryObject<T> Take(int count)
        {
            var generics = new Type[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            this.AddMethodExpression("Take", generics, parameters);
            return this;
        }

        public IQueryObject<T> Skip(int count)
        {
            var generics = new Type[] { typeof(T) };
            var parameters = new Expression[] { Expression.Constant(count) };
            this.AddMethodExpression("Skip", generics, parameters);
            return this;
        }

        #region Helper methods
        
        static ReadOnlyCollection<MethodInfo> QueryableMethods;
        static QueryObject()
        {
            QueryableMethods = new ReadOnlyCollection<MethodInfo>(typeof(System.Linq.Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static).ToList()); 
        }

        List<Tuple<MethodInfo, Expression[]>> _expressionList = new List<Tuple<MethodInfo, Expression[]>>();
        private void AddMethodExpression(string methodName, Type[] generics, Expression[] parameters)
        {
            MethodInfo orderMethodInfo = QueryableMethods
                .Where(m => m.Name == methodName && m.GetParameters().Length == parameters.Length + 1).First();

            orderMethodInfo = orderMethodInfo.MakeGenericMethod(generics);
            _expressionList.Add(new Tuple<MethodInfo, Expression[]>(orderMethodInfo, parameters));
        }

        private IQueryable<T> AppendExpressions(IQueryable<T> query)
        {
            var source = query;
            foreach (var exp in _expressionList)
            {
                var newParams = exp.Item2.ToList();
                newParams.Insert(0, source.Expression);
                source = source.Provider.CreateQuery<T>(Expression.Call(null, exp.Item1, newParams));
            }
            return source;
        }

        #endregion
    }
}
