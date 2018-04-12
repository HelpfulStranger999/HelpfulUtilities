using HelpfulUtilities.Extensions;
using System;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using System.Collections.Generic;

namespace Discord.Addons.Listeners
{

    public class DependencyManager
    {
        public static DependencyFactory Factory = new DependencyFactory();

        private IServiceProvider _services;
        private object[] _deps;
        private Type _type;

        private DependencyManager(DependencyFactory factory)
        {
            _services = factory.ServiceProvider;
            _deps = factory.Dependencies.ToArray();
            _type = factory.Type;
        }

        public T InvokeMethod<T>(T obj, MethodInfo method)
            => (T)InvokeMethod((object)obj, method);

        public object InvokeMethod(object obj, MethodInfo method)
        {
            var args = Inject(method);
            if (args.Any(o => o == null)) return null;
            return method.Invoke(obj, args);
        }

        public T CreateObject<T>()
            => (T)CreateObject();

        public object CreateObject()
        {
            var obj = ChooseConstructor(_type);
            Inject(obj);
            return obj;
        }

        public T InvokeConstructor<T>(ConstructorInfo constructor)
            => (T)InvokeConstructor(constructor);

        public object InvokeConstructor(ConstructorInfo constructor)
        {
            var args = Inject(constructor);
            if (args.Any(obj => obj == null)) return null;
            return constructor.Invoke(args);
        }

        public object ChooseConstructor()
        {
            var constructors = _type.GetConstructors();

            if (constructors.Length == 0)
                throw new InvalidOperationException($"No constructors were found for {_type.Name}");

            if (constructors.Count() == 1)
                return InvokeConstructor(constructors[0]);

            foreach (var constructor in constructors.OrderBy(constructor => constructor.GetParameters().Count()))
            {
                var invoked = InvokeConstructor(constructor);
                if (invoked != null) return invoked;
            }

            throw new InvalidOperationException($"Failed to create {_type.Name}, no constructor found!");
        }

        public void Inject(object obj)
        {
            var properties = obj.GetType().GetProperties().Where(property =>
            {
                return property.GetCustomAttribute<DontInjectAttribute>() == null;
            });

            foreach (var property in properties)
                property.SetValue(obj, RetrieveValue(obj.GetType(), property.PropertyType));
        }

        public object[] Inject(MethodBase method)
        {
            var parameters = method.GetParameters();
            var arguments = new object[parameters.Count()];

            for (int i = 0; i < arguments.Length; i++)
                arguments[i] = RetrieveValue(parameters[i].ParameterType);

            return arguments;
        }

        public object RetrieveValue(Type type)
        {
            if (type.Extends(typeof(IServiceProvider)))
                return _services;

            var dependency = _deps.FirstOrDefault(obj =>
            {
                return type.Extends(obj.GetType());
            }) ?? _services.GetService(type);

            if (dependency == null && type.GetCustomAttribute<OptionalDependencyAttribute>() == null)
            {
                throw new InvalidOperationException($"Failed to create {_type.Name}, required dependency {type.Name} could not be found.");
            }

            return dependency;
        }

        public class DependencyFactory
        {
            public IServiceProvider ServiceProvider { get; set; } = new EmptyServiceProvider();
            public ICollection<object> Dependencies { get; set; } = new List<object>();
            public Type Type { get; set; }

            private DependencyManager _manager = null;

            public DependencyFactory WithServiceProvider(IServiceProvider serviceProvider)
            {
                ServiceProvider = serviceProvider;
                return this;
            }

            public DependencyFactory SetDependencies(params object[] dependencies)
                => SetDependencies(dependencies);

            public DependencyFactory SetDependencies(ICollection<object> dependencies)
            {
                Dependencies = dependencies;
                return this;
            }

            public DependencyFactory WithDependencies(params object[] dependencies)
            {
                Dependencies.AddRange(dependencies);
                return this;
            }

            public DependencyFactory WithDependency(object dependency)
            {
                Dependencies.Add(dependency);
                return this;
            }

            public DependencyFactory WithType(Type type)
            {
                Type = type;
                return this;
            }

            public DependencyFactory WithType<T>()
            {
                Type = typeof(T);
                return this;
            }

            public DependencyManager Create()
            {
                if (_manager == null) _manager = new DependencyManager(this);

                if (_manager._type != Type ||
                    _manager._services != ServiceProvider ||
                    _manager._deps != Dependencies)
                {
                    _manager = new DependencyManager(this);
                }

                return _manager;
            }
        }

    }

    internal class EmptyServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType) => null;
    }
}
