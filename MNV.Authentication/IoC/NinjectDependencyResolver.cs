using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using System.Web.Http.Dependencies;

namespace MNV.Infrastructure.IoC
{
    public class NinjectDependencyResolver : NinjectDependencyScope
        ,System.Web.Http.Dependencies.IDependencyResolver
        ,System.Web.Mvc.IDependencyResolver
    {
        public readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(_kernel.BeginBlock());
        }

        //public object GetService(Type serviceType)
        //{
        //    return _kernel.TryGet(serviceType);
        //}

        //public IEnumerable<object> GetServices(Type serviceType)
        //{
        //    return _kernel.GetAll(serviceType);
        //}

        //public void Dispose()
        //{
        //    GC.SuppressFinalize(this);
        //}
    }
}
