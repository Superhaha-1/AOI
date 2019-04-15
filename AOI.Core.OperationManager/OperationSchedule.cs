using AOI.Core.Interfaces;
using System.Collections.Generic;
using Splat;
using Autofac;
using AOI.Core.OperationManager.Interfaces;

namespace AOI.Core.OperationManager
{
    public sealed class OperationSchedule : IOperationInitializer, IOperationInvoker, IOperationBuilderDictionary, IEnableLogger
    {
        #region 实现IOperationBuilderDictionary

        IReadOnlyDictionary<string, IOperationBuilder> IOperationBuilderDictionary.Operations => _operationBuilderDictionary;

        #endregion

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
                this.Log().Warn($"操作-{operationName}-已存在");
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
                            this.Log().Info($"操作-{name}-参数-{name}-不支持");
                            return;
                        }
                    }
                operation.Make();
            }
            else
            {
                this.Log().Info($"操作-{name}-不支持");
            }
        }
    }
}
