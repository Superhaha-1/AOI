using AOI.Core.Interfaces;
using System;
using System.Collections.Generic;
using ReactiveUI;
using Splat;

namespace AOI.Core.CommandManager
{
    public sealed class CommandInitialize : ICommandInitialize, IEnableLogger
    {
        private readonly Dictionary<string, ReactiveCommand<IDictionary<string, object>, IOperation>> _commandDictionary = new Dictionary<string, ReactiveCommand<IDictionary<string, object>, IOperation>>();

        void ICommandInitialize.InitializeCommand<T>(string commandName, IDictionary<string, Action<T, object>> setters)
        {
            commandName = commandName.ToLower();
            if (_commandDictionary.ContainsKey(commandName))
                this.Log().Error("已存在该命令");
            _commandDictionary.Add(commandName, ReactiveCommand.Create<IDictionary<string, object>, IOperation>(parameters =>
            {
                var operation = new T();
                if (parameters == null)
                    return operation;
                foreach (var keypair in parameters)
                {
                    if (setters.TryGetValue(keypair.Key, out var setter))
                    {
                        setter.Invoke(operation, keypair.Value);
                    }
                    else
                    {
                        this.Log().Error("不存在该参数");
                    }
                }
                return operation;
            }));
        }
    }
}
