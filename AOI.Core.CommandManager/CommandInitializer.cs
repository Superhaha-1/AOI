using AOI.Core.Interfaces;
using System.Collections.Generic;
using ReactiveUI;
using Splat;
using System.Linq;

namespace AOI.Core.CommandManager
{
    public sealed class CommandInitializer : ICommandInitializer, IEnableLogger
    {
        private readonly Dictionary<string, CommandDecorator> _commandDecoratorDictionary = new Dictionary<string, CommandDecorator>();

        void ICommandInitializer.InitializeCommand<T>(ICommandBuilder<T> commandBuilder)
        {
            var commandName = commandBuilder.Name.ToLower();
            if (_commandDecoratorDictionary.ContainsKey(commandName))
            {
                this.Log().Error("已存在该命令");
                return;
            }
            _commandDecoratorDictionary.Add(commandName, new CommandDecorator(commandName,
                commandBuilder.ParameterBuilderDictionary.Values.ToDictionary(builder => builder.Name, builder => new CommandParameterDecorator(builder.Name, builder.HelpMessage)), ReactiveCommand.Create<IDictionary<string, object>, IOperation>(parameters =>
                 {
                     var t = new T();
                     if (parameters == null)
                         return t;
                     foreach (var keypair in parameters)
                     {
                         if (commandBuilder.ParameterBuilderDictionary.TryGetValue(keypair.Key, out var builder))
                         {
                             builder.Set(t, keypair.Value);
                         }
                         else
                         {
                             this.Log().Error("不存在该参数");
                         }
                     }
                     return t;
                 })));
        }
    }

    public sealed class CommandDecorator
    {
        public CommandDecorator(string name, Dictionary<string, CommandParameterDecorator> parameterDecoratorDictionary, ReactiveCommand<IDictionary<string, object>, IOperation> command)
        {
            Name = name;
            ParameterDecoratorDictionary = parameterDecoratorDictionary;
            Command = command;
        }

        public string Name { get; }

        public Dictionary<string, CommandParameterDecorator> ParameterDecoratorDictionary { get; }

        ReactiveCommand<IDictionary<string, object>, IOperation> Command { get; }
    }

    public sealed class CommandParameterDecorator
    {
        public CommandParameterDecorator(string name, string helpMessage)
        {
            Name = name;
            HelpMessage = helpMessage;
        }

        public string Name { get; }

        public string HelpMessage { get; }
    }
}
