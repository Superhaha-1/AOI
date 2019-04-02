using AOI.Core.Interfaces;
using System.Collections.Generic;
using Splat;

namespace AOI.Core.CommandManager
{
    public sealed class CommandSchedule : ICommandInitializer, ICommandInvoker, IEnableLogger
    {
        private readonly Dictionary<string, ICommandBuilder> _commandBuilderDictionary = new Dictionary<string, ICommandBuilder>();

        public CommandSchedule()
        {

        }

        void ICommandInitializer.InitializeCommand(ICommandBuilder commandBuilder)
        {
            var commandName = commandBuilder.Name.ToLower();
            if (_commandBuilderDictionary.ContainsKey(commandName))
            {
                this.Log().Info($"已存在命令-{commandName},无法初始化");
                return;
            }
            _commandBuilderDictionary.Add(commandName, commandBuilder);
        }

        void ICommandInvoker.InvokeCommand(string name, params (string Name, object Value)[] parameters)
        {
            name = name.ToLower();
            if (_commandBuilderDictionary.TryGetValue(name, out var commandBuilder))
            {
                var operation = commandBuilder.CreateOperation();
                if (parameters != null)
                    foreach (var parameter in parameters)
                    {
                        var parameterName = parameter.Name;
                        if (commandBuilder.ParameterBuilderDictionary.TryGetValue(parameterName, out var commandParameterBuilder))
                        {
                            commandParameterBuilder.Set(operation, parameter.Value);
                        }
                        else
                        {
                            this.Log().Info($"不支持的参数-{name}");
                            return;
                        }
                    }
                operation.Redo();
            }
            else
            {
                this.Log().Info($"不支持的命令-{name}");
            }
        }
    }
}
