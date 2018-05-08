using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace HelpfulUtilities.Discord.Listeners
{
    /// <summary>Provides info and execution info of this listener.</summary>
    public class ListenerInfo
    {
        private ListenerAttribute _listener;
        private MethodInfo _method;

        /// <summary>The <see cref="ListenerService"/> of this listener.</summary>
        public ListenerService Service { get; }

        /// <summary>The run mode of this listener</summary>
        public RunMode RunMode { get; }

        /// <summary>The name of this listener; defaults to method name.</summary>
        public string Name { get; }

        /// <summary>The summary of this listener or null.</summary>
        public string Summary { get; }

        /// <summary>Any remarks on this listener or null.</summary>
        public string Remarks { get; }

        /// <summary>An immutable list of attributes</summary>
        public IReadOnlyList<Attribute> Attributes { get; }

        internal ListenerInfo(ListenerService service, MethodInfo method)
        {
            _listener = method.GetCustomAttribute<ListenerAttribute>();
            _method = method;

            Service = service;
            RunMode = _listener.RunMode == RunMode.Default ? service._runMode : _listener.RunMode;

            Name = method.GetCustomAttribute<NameAttribute>()?.Text ?? method.Name;
            Summary = method.GetCustomAttribute<SummaryAttribute>()?.Text ?? null;
            Remarks = method.GetCustomAttribute<RemarksAttribute>()?.Text ?? null;

            Attributes = method.GetCustomAttributes().Where(a =>
            {
                return a is NameAttribute || a is SummaryAttribute || a is RemarksAttribute;
            }).ToImmutableArray();
        }

        /// <summary>Checks the builtin preconditions of the listener and returns the result</summary>
        /// <param name="context">The context of the listener</param>
        public IResult CheckPreconditions(ICommandContext context)
        {
            var result = _listener.HasRequiredContext(context);
            if (!result.IsSuccess)
                return result;

            result = _listener.IsFromRequiredUser(context);
            if (!result.IsSuccess)
                return result;

            return PreconditionResult.FromSuccess();
        }

        /// <summary>Executes this listener</summary>
        /// <param name="context">The context of the listeners</param>
        /// <param name="services">The <see cref="IServiceProvider"/> to use to inject dependencies</param>
        public IResult Execute(ICommandContext context, IServiceProvider services)
        {
            var manager = DependencyManager.Factory.SetDependencies(context)
                .WithServiceProvider(services)
                .WithType(_method.DeclaringType)
                .Build();

            var listener = manager.CreateObject();
            if (listener is ModuleBase<ICommandContext> module)
            {
                listener = manager.InjectPrivateProperty(listener, context, nameof(module.Context), module.GetType());
            }

            switch (RunMode)
            {
                case RunMode.Sync:
                    return ExecuteInternal(manager, listener);
                case RunMode.Async:
                    _ = Task.Run(() => ExecuteInternal(manager, listener)).ConfigureAwait(false);
                    break;
            }

            return ExecuteResult.FromSuccess();
        }

        internal ExecuteResult ExecuteInternal(DependencyManager manager, object listener)
        {
            try
            {
                manager.InvokeMethod(listener, _method);
                return ExecuteResult.FromSuccess();
            }
            catch (TargetInvocationException e)
            {
                return ExecuteResult.FromError(e.InnerException);
            }
        }
    }
}
