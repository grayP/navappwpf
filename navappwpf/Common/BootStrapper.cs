
using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using navappwpf.Common.Ioc;
using NmeaParser.Navigate;

namespace navappwpf.Common
{
    public static class BootStrapper
    {
        private static ILifetimeScope _rootScope;
        public static void Start()
        {
            if (_rootScope != null) return;
            var builder = new ContainerBuilder();

            builder.RegisterInstance<IDispatcher>(new DispatcherWrapper());

 
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => typeof(IService).IsAssignableFrom(t))
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => typeof(IViewModel).IsAssignableFrom(t) && !typeof(ITransientViewModel).IsAssignableFrom(t))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(t => typeof(ITransientViewModel).IsAssignableFrom(t))
                .Where(t =>
                {
                    var isAssignable = typeof(ITransientViewModel).IsAssignableFrom(t);
                    if (isAssignable)
                    {
                      //  Debug.WriteLine($"Transient View model-{t.Name}");
                    }
                    return isAssignable;
                })
                .AsImplementedInterfaces()
                .ExternallyOwned();
            _rootScope = builder.Build();
        }
        public static void Stop()
        {
            _rootScope.Dispose();
        }
        public static T Resolve<T>()
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(new Parameter[0]);
        }
        public static T Resolve<T>(Parameter[] parameters)
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(parameters);
        }

    }

}
