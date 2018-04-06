using HelpfulUtilities.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace HelpfulUtilities.Discord.Commands
{
    /// <summary>This class provides static methods for creating and injecting dependencies.</summary>
    public static class DependencyInjection
    {
        /// <summary>This method creates an object and injects dependencies.</summary>
        /// <typeparam name="T">Object type to create and inject dependencies into</typeparam>
        /// <param name="services">The <see cref="IServiceProvider"/> to collect most dependencies from</param>
        /// <param name="dependencies">Extraneous dependencies not in <see cref="IServiceProvider"/> to collect dependencies from</param>
        /// <returns>A new object injected with dependencies.</returns>
        public static T CreateInjected<T>(IServiceProvider services, params object[] dependencies)
            => CreateInjected<T>(typeof(T), services, dependencies);

        /// <summary>This method creates an object and injects dependencies.</summary>
        /// <typeparam name="T">Object type to create and inject dependencies into</typeparam>
        /// <param name="type">The <see cref="Type"/> of the object</param>
        /// <param name="services">The <see cref="IServiceProvider"/> to collect most dependencies from</param>
        /// <param name="dependencies">Extraneous dependencies not in <see cref="IServiceProvider"/> to collect dependencies from</param>
        /// <returns>A new object injected with dependencies.</returns>
        public static T CreateInjected<T>(Type type, IServiceProvider services, params object[] dependencies)
        {
            var constructor = type.GetConstructors().First() ?? type.GetConstructor(Type.EmptyTypes);
            var parameters = constructor.GetParameters();
            var args = new object[parameters.Count()];

            for (int i = 0; i < args.Count(); i++)
            {
                var param = parameters[i];
                if (param.ParameterType.GetInterfaces().Contains(typeof(IServiceProvider)))
                {
                    args[i] = services;
                    continue;
                }

                var dependency = dependencies.Where(o => param.Extends(o.GetType()));
                if (dependency.Count() > 0)
                {
                    args[i] = dependency.Take(1);
                    continue;
                }

                args[i] = services.GetRequiredService(param.ParameterType);
            }

            return Inject((T)constructor.Invoke(args), services, dependencies);
        }
        
        /// <summary>This method injects an object with dependencies.</summary>
        /// <typeparam name="T">Object type to inject dependencies into</typeparam>
        /// <param name="obj">Object to inject dependencies into</param>
        /// <param name="services">The <see cref="IServiceProvider"/> to collect most dependencies from</param>
        /// <param name="dependencies">Extraneous dependencies not in <see cref="IServiceProvider"/> to collect dependencies from</param>
        /// <returns>The object injected with dependencies.</returns>
        public static T Inject<T>(T obj, IServiceProvider services, params object[] dependencies)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (property.Extends(services.GetType()))
                {
                    property.SetValue(obj, services);
                    continue;
                }

                var dependency = dependencies.Where(o => property.Extends(o.GetType()));
                if (dependency.Count() > 0)
                {
                    property.SetValue(obj, dependency.Take(1));
                    continue;
                }

                property.SetValue(obj, services.GetRequiredService(property.PropertyType));

            }

            return obj;
        }
    }
}
