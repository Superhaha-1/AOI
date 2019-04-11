using AOI.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using Splat;

namespace AOI.Core.Extensions
{
    public static class OperationExtensions
    {
        public static OperationBuilder<TOperation> CreateOperationBuilder<TOperation>(this ContainerBuilder containerBuilder, Func<IComponentContext, TOperation> ctor, string name = null, string description = null) where TOperation : IOperation, new()
        {
            return new OperationBuilder<TOperation>(containerBuilder, ctor, name, description);
        }

        public static OperationParameterBuilder<TOperation, TValue> CreateParameterBuilder<TOperation, TValue>(this OperationBuilder<TOperation> operationBuilder, string name) where TOperation : IOperation, new()
        {
            OperationParameterBuilder<TOperation, TValue> parameterBuilder;
            if (operationBuilder.ParameterBuilderDictionary.TryGetValue(name, out var builder))
            {
                parameterBuilder = builder as OperationParameterBuilder<TOperation, TValue>;
                LogHost.Default.Warn($"操作-{operationBuilder.Name}-已存在参数-{name}");
                parameterBuilder.OperationBuilder = operationBuilder;
            }
            else
            {
                parameterBuilder = new OperationParameterBuilder<TOperation, TValue>(operationBuilder, name);
                operationBuilder.ParameterBuilderDictionary.Add(name, parameterBuilder);
            }
            return parameterBuilder;
        }

        public static OperationParameterBuilder<TOperation, TValue> SetDescription<TOperation, TValue>(this OperationParameterBuilder<TOperation, TValue> parameterBuilder, string description) where TOperation : IOperation, new()
        {
            parameterBuilder.Description = description;
            return parameterBuilder;
        }

        public static IOperationBuilder Build<TOperation, TValue>(this OperationParameterBuilder<TOperation, TValue> parameterBuilder, Action<TOperation, TValue> action) where TOperation : IOperation, new()
        {
            parameterBuilder.SetAction(action);
            var operationBuilder = parameterBuilder.OperationBuilder;
            parameterBuilder.OperationBuilder = null;
            return operationBuilder;
        }

        public static void Build<TOperation>(this OperationBuilder<TOperation> operationBuilder) where TOperation : IOperation, new()
        {
            var containerBuilder = operationBuilder.ContainerBuilder;
            operationBuilder.ContainerBuilder = null;
            containerBuilder.RegisterBuildCallback(c => c.Resolve<IOperationInitializer>().InitializeOperation(operationBuilder));
        }
    }

    public sealed class OperationBuilder<TOperation> : IOperationBuilder where TOperation : IOperation, new()
    {
        internal OperationBuilder(ContainerBuilder containerBuilder, Func<IComponentContext, TOperation> ctor, string name, string description)
        {
            ContainerBuilder = containerBuilder;
            _ctor = ctor;
            Name = (name ?? typeof(TOperation).Name).ToUpperInvariant();
            Description = description;
        }

        private readonly Func<IComponentContext, TOperation> _ctor;

        internal ContainerBuilder ContainerBuilder { get; set; }

        internal string Name { get; }

        string IOperationBuilder.Name => Name;

        internal string Description { get; }

        string IOperationBuilder.Description => Description;

        internal Dictionary<string, IOperationParameterBuilder> ParameterBuilderDictionary { get; } = new Dictionary<string, IOperationParameterBuilder>();

        IDictionary<string, IOperationParameterBuilder> IOperationBuilder.ParameterBuilderDictionary => ParameterBuilderDictionary;

        IOperation IOperationBuilder.CreateOperation(IComponentContext componentContext)
        {
            return _ctor.Invoke(componentContext);
        }
    }

    public sealed class OperationParameterBuilder<TOperation, TValue> : IOperationParameterBuilder, IEnableLogger where TOperation : IOperation, new()
    {
        internal OperationParameterBuilder(OperationBuilder<TOperation> operationBuilder, string name)
        {
            OperationBuilder = operationBuilder;
            Name = name?.ToUpperInvariant();
        }

        internal OperationBuilder<TOperation> OperationBuilder { get; set; }

        internal string Name { get; }

        string IOperationParameterBuilder.Name => Name;

        internal string Description { get; set; }

        string IOperationParameterBuilder.Description => Description;

        private Action<TOperation, TValue> _action;

        internal void SetAction(Action<TOperation, TValue> action)
        {
            _action = action;
        }

        void IOperationParameterBuilder.SetParameter(IOperation operation, object parameter)
        {
            if (operation == null)
            {
                this.Log().Info($"Operation为空");
                return;
            }
            if (operation is TOperation t)
            {
                if (parameter is TValue m)
                {
                    _action.Invoke(t, m);
                }
                else
                {
                    this.Log().Info($"Parameter类型不为{typeof(TValue)}");
                }
            }
            else
            {
                this.Log().Info($"Operation类型不为{typeof(TOperation)}");
            }
        }
    }
}
