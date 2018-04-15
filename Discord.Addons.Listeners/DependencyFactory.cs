using HelpfulUtilities.Extensions;
using System;
using System.Linq;
using System.Reflection;
using Discord.Commands;
using System.Collections.Generic;

namespace HelpfulUtilities.Discord.Listeners
{
    /// <summary>Handles creation and injection of dependencies</summary>
    public class DependencyManager
    {
        /// <summary>Static instance of <see cref="DependencyFactory"/></summary>
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

        /// <summary>Invokes a specified method on a given object of type <typeparamref name="T"/></summary>
        public T InvokeMethod<T>(T obj, MethodInfo method)
            => (T)InvokeMethod((object)obj, method);

        /// <summary>Invokes a specified method on a given object</summary>
        public object InvokeMethod(object obj, MethodInfo method)
        {
            var args = Inject(method);
            if (args.Any(o => o == null)) return null;
            return method.Invoke(obj, args);
        }

        /// <summary>Initializes an object of type <typeparamref name="T"/></summary>
        public T CreateObject<T>()
            => (T)CreateObject();

        /// <summary>Initializes an object</summary>
        public object CreateObject()
        {
            var obj = ChooseConstructor();
            Inject(obj);
            return obj;
        }

        /// <summary>Invokes a specified constructor of type <typeparamref name="T"/></summary>
        /// <param name="constructor">The constructor to invoke</param>
        public T InvokeConstructor<T>(ConstructorInfo constructor)
            => (T)InvokeConstructor(constructor);

        /// <summary>Invokes a specified constructor</summary>
        /// <param name="constructor">The constructor to invoke</param>
        public object InvokeConstructor(ConstructorInfo constructor)
        {
            var args = Inject(constructor);
            return constructor.Invoke(args);
        }

        /// <summary>Chooses the best constructor it can invoke</summary>
        public object ChooseConstructor()
        {
            var constructors = _type.GetConstructors();

            if (constructors.Length == 0)
                throw new InvalidOperationException($"No constructors were found for {_type.Name}");

            if (constructors.Count() == 1)
                return InvokeConstructor(constructors[0]);

            foreach (var constructor in constructors.OrderBy(constructor => constructor.GetParameters().Count()))
            {
                try
                {
                    return InvokeConstructor(constructor);
                }
                catch (InvalidOperationException)
                {
                    continue;
                }
            }

            throw new InvalidOperationException($"Failed to create {_type.Name}, no constructor found!");
        }

        /// <summary>Injects dependencies into the properties of the specified object</summary>
        public void Inject(object obj)
        {
            var properties = obj.GetType().GetProperties().Where(property =>
            {
                return property.GetCustomAttribute<DontInjectAttribute>() == null;
            });

            foreach (var property in properties)
                property.SetValue(obj, RetrieveValue(property.PropertyType));
        }

        /// <summary>Returns a list of dependencies as detailed by the <paramref name="method"/> passed</summary>
        public object[] Inject(MethodBase method)
        {
            var parameters = method.GetParameters();
            var arguments = new object[parameters.Count()];

            for (int i = 0; i < arguments.Length; i++)
                arguments[i] = RetrieveValue(parameters[i].ParameterType);

            return arguments;
        }

        /// <summary>Returns the required dependency of the specified type.</summary>
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

        /// <summary>Represents a factory for creating <see cref="DependencyManager"/></summary>
        public class DependencyFactory
        {
            /// <summary>The <see cref="IServiceProvider"/> to use for dependency injection</summary>
            public IServiceProvider ServiceProvider { get; set; } = new EmptyServiceProvider();

            /// <summary>Miscellaneous dependencies not found in <see cref="IServiceProvider"/></summary>
            public ICollection<object> Dependencies { get; set; } = new List<object>();
            
            /// <summary>The type to create, inject, and invoke.</summary>
            public Type Type { get; set; }

            private DependencyManager _manager = null;

            /// <summary>Sets the service provider</summary>
            public DependencyFactory WithServiceProvider(IServiceProvider serviceProvider)
            {
                ServiceProvider = serviceProvider;
                return this;
            }

            /// <summary>Sets the dependencies</summary>
            public DependencyFactory SetDependencies(params object[] dependencies)
                => SetDependencies(dependencies);

            /// <summary>Sets the dependencies</summary>
            public DependencyFactory SetDependencies(ICollection<object> dependencies)
            {
                Dependencies = dependencies;
                return this;
            }

            /// <summary>Appends dependencies</summary>
            public DependencyFactory WithDependencies(params object[] dependencies)
            {
                Dependencies.AddRange(dependencies);
                return this;
            }

            /// <summary>Appends a dependency</summary>
            public DependencyFactory WithDependency(object dependency)
            {
                Dependencies.Add(dependency);
                return this;
            }

            /// <summary>Sets the type</summary>
            public DependencyFactory WithType(Type type)
            {
                Type = type;
                return this;
            }

            /// <summary>Sets the type</summary>
            public DependencyFactory WithType<T>()
            {
                Type = typeof(T);
                return this;
            }

            /// <summary>Builds a <see cref="DependencyManager"/></summary>
            public DependencyManager Build()
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
