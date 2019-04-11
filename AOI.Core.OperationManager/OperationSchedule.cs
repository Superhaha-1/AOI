using AOI.Core.Interfaces;
using System.Collections.Generic;
using Splat;
using Autofac;

namespace AOI.Core.OperationManager
{
    public sealed class OperationSchedule : IOperationInitializer, IOperationInvoker, IEnableLogger
    {
        private readonly Dictionary<string, IOperationBuilder> _operationBuilderDictionary = new Dictionary<string, IOperationBuilder>();

        private readonly IComponentContext _componentContext;

        public OperationSchedule(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        void IOperationInitializer.InitializeOperation(IOperationBuilder operationBuilder)
        {
            var operationName = operationBuilder.Name;
            if (_operationBuilderDictionary.ContainsKey(operationName))
            {
                this.Log().Warn($"已存在命令-{operationName},无法初始化");
                return;
            }
            _operationBuilderDictionary.Add(operationName, operationBuilder);
        }

        void IOperationInvoker.InvokeOperation(string name, params (string Name, object Value)[] parameters)
        {
            name = name.ToUpperInvariant();
            if (_operationBuilderDictionary.TryGetValue(name, out var operationBuilder))
            {
                var operation = operationBuilder.CreateOperation(_componentContext);
                if (parameters != null)
                    foreach (var (Name, Value) in parameters)
                    {
                        var parameterName = Name;
                        if (operationBuilder.ParameterBuilderDictionary.TryGetValue(parameterName, out var operationParameterBuilder))
                        {
                            operationParameterBuilder.SetParameter(operation, Value);
                        }
                        else
                        {
                            this.Log().Info($"不支持的参数-{name}");
                            return;
                        }
                    }
                operation.Make();
            }
            else
            {
                this.Log().Info($"不支持的命令-{name}");
            }
        }
    }
}
