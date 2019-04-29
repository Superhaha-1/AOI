using AOI.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using Splat;

namespace AOI.Core.Extensions
{
    public static class OperationExtensions
    {
        public static OperationBuilder<TOperation> CreateOperationBuilder<TOperation>(this ContainerBuilder containerBuilder, Func<IComponentContext, TOperation> ctor, string name = null, string description = null) where TOperation : IOperation
        {
            return new OperationBuilder<TOperation>(containerBuilder, ctor, name, description);
        }

        public static OperationBuilder<TOperation> SetOperationParameter<TOperation, TValue>(this OperationBuilder<TOperation> operationBuilder, string name, Action<TOperation, TValue> set, string description = null) where TOperation : IOperation
        {
            if (operationBuilder.ParameterBuilderDictionary.ContainsKey(name))
            {
                throw new Exception($"操作-{operationBuilder.Name}-已存在参数-{name}");
            }
            if(set == null)
            {
                throw new Exception($"操作-{operationBuilder.Name}-参数-{name}-没有可用的Set");
            }
            var parameterBuilder = new OperationParameterBuilder<TOperation, TValue>(name, set, description);
            operationBuilder.ParameterBuilderDictionary.Add(name, parameterBuilder);
            return operationBuilder;
        }

        public static void Build<TOperation>(this OperationBuilder<TOperation> operationBuilder) where TOperation : IOperation
        {
            if(!operationBuilder.HasCtor)
            {
                throw new Exception($"操作-{operationBuilder.Name}-没有可用的Ctor");
            }
            var containerBuilder = operationBuilder.ContainerBuilder;
            operationBuilder.ContainerBuilder = null;
            containerBuilder.RegisterBuildCallback(c => c.Resolve<IOperationInitializer>().InitializeOperation(operationBuilder));
        }
    }

    public sealed class OperationBuilder<TOperation> : IOperationBuilder, IEnableLogger where TOperation : IOperation
    {
        internal OperationBuilder(ContainerBuilder containerBuilder, Func<IComponentContext, TOperation> ctor, string name, string description)
        {
            ContainerBuilder = containerBuilder;
            _ctor = ctor;
            Name = (name ?? typeof(TOperation).Name).ToUpperInvariant();
            _description = description;
        }

        private readonly Func<IComponentContext, TOperation> _ctor;

        internal bool HasCtor => _ctor != null;

        internal ContainerBuilder ContainerBuilder { get; set; }

        internal string Name { get; }

        string IOperationBuilder.Name => Name;

        private readonly string _description;

        string IOperationBuilder.Description => _description;

        internal Dictionary<string, IOperationParameterBuilder> ParameterBuilderDictionary { get; } = new Dictionary<string, IOperationParameterBuilder>();

        IDictionary<string, IOperationParameterBuilder> IOperationBuilder.ParameterBuilderDictionary => ParameterBuilderDictionary;

        IOperation IOperationBuilder.CreateOperation(IComponentContext componentContext)
        {
            return _ctor.Invoke(componentContext);
        }
    }

    public sealed class OperationParameterBuilder<TOperation, TValue> : IOperationParameterBuilder, IEnableLogger where TOperation : IOperation
    {
        internal OperationParameterBuilder(string name, Action<TOperation, TValue> set, string description)
        {
            _name = name?.ToUpperInvariant();
            _set = set;
            _description = description;
        }

        private readonly string _name;

        string IOperationParameterBuilder.Name => _name;

        private readonly string _description;

        string IOperationParameterBuilder.Description => _description;

        private readonly Action<TOperation, TValue> _set;

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
                    _set.Invoke(t, m);
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
