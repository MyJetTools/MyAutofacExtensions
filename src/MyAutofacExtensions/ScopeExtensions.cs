using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace MyAutofacExtensions
{
    public static class ScopeExtensions
    {
        public static void TestAutofacDependency(IEnumerable<IModule> modules)
        {
            var builder = new ContainerBuilder();

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            var container = builder.Build();

            var scope = container.BeginLifetimeScope();

            var ignoredAssemblies = new List<string>()
            {
                //"System.Object"
            };

            var list = scope.ResolveAll(ignoredAssemblies);
        }

        public static IList<IServiceWithType> Filter(this IEnumerable<IServiceWithType> services,
            IEnumerable<string> ignoredAssemblies)
        {
            return services.Where(serviceWithType => ignoredAssemblies
                .All(ignored => ignored != serviceWithType.ServiceType.FullName)).ToList();
        }

        public static IList<object> ResolveAll(this ILifetimeScope scope, IEnumerable<string> ignoredAssemblies)
        {
            var services = scope.ComponentRegistry.Registrations.SelectMany(x => x.Services)
                .OfType<IServiceWithType>().Filter(ignoredAssemblies).ToList();

            foreach (var serviceWithType in services)
            {
                try
                {
                    scope.Resolve(serviceWithType.ServiceType);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return services.Select(x => x.ServiceType).Select(scope.Resolve).ToList();
        }
    }
}
