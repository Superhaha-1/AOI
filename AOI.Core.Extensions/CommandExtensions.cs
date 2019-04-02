using AOI.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using Splat;

namespace AOI.Core.Extensions
{
    public static class CommandExtensions
    {
        public static ICommandBuilder<T> CreateCommandBuilder<T>(this ContainerBuilder containerBuilder) where T : IOperation, new()
        {
            return new CommandBuilder<T>(containerBuilder);
        }

        public static ICommandBuilder<T> Named<T>(this ICommandBuilder<T> commandBuilder, string name) where T : IOperation, new()
        {
            commandBuilder.Name = name;
            return commandBuilder;
        }

        public static ICommandParameterBuilder<T, M> SetParameter<T, M>(this ICommandBuilder commandBuilder) where T : IOperation, new()
        {
            return new CommandParameterBuilder<T, M>(commandBuilder);
        }

        public static ICommandParameterBuilder<T, M> Named<T, M>(this ICommandParameterBuilder<T, M> commandParameterBuilder, string name) where T : IOperation, new()
        {
            commandParameterBuilder.Name = name;
            return commandParameterBuilder;
        }

        public static ICommandParameterBuilder<T, M> SetHelpMessage<T, M>(this ICommandParameterBuilder<T, M> commandParameterBuilder, string message) where T : IOperation, new()
        {
            commandParameterBuilder.HelpMessage = message;
            return commandParameterBuilder;
        }

        public static ICommandBuilder UpDate<T, M>(this ICommandParameterBuilder<T, M> commandParameterBuilder, Action<T, M> action) where T : IOperation, new()
        {
            commandParameterBuilder.Action = action;
            var commandBuilder = commandParameterBuilder.CommandBuilder;
            var dictionary = commandBuilder.ParameterBuilderDictionary;
            var name = commandParameterBuilder.Name;
            if (dictionary.ContainsKey(name))
            {
                dictionary[name] = commandParameterBuilder;
            }
            else
            {
                dictionary.Add(name, commandParameterBuilder);
            }
            return commandBuilder;
        }

        public static void Build<T>(this ICommandBuilder<T> commandBuilder) where T : IOperation, new()
        {
            commandBuilder.ContainerBuilder.RegisterBuildCallback(c => c.Resolve<ICommandInitializer>().InitializeCommand(commandBuilder));
        }
    }

    public interface ICommandBuilder<T> : ICommandBuilder where T : IOperation, new()
    {
        ContainerBuilder ContainerBuilder { get; }
    }

    internal sealed class CommandBuilder<T> : ICommandBuilder<T> where T : IOperation, new()
    {
        public CommandBuilder(ContainerBuilder containerBuilder)
        {
            ContainerBuilder = containerBuilder;
            Name = typeof(T).Name;
        }

        public ContainerBuilder ContainerBuilder { get; }

        public string Name { get; set; }

        public IDictionary<string, ICommandParameterBuilder> ParameterBuilderDictionary { get; } = new Dictionary<string, ICommandParameterBuilder>();

        public IOperation CreateOperation()
        {
            return new T();
        }
    }

    public interface ICommandParameterBuilder<T, M> : ICommandParameterBuilder where T : IOperation, new()
    {
        ICommandBuilder CommandBuilder { get; }

        Action<T, M> Action { set; }
    }

    internal sealed class CommandParameterBuilder<T, M> : ICommandParameterBuilder<T, M>, IEnableLogger where T : IOperation, new()
    {
        public CommandParameterBuilder(ICommandBuilder commandBuilder)
        {
            CommandBuilder = commandBuilder;
        }

        public ICommandBuilder CommandBuilder { get; }

        public string Name { get; set; }

        public string HelpMessage { get; set; }

        public Action<T, M> Action { private get; set; }

        public void Set(IOperation operation, object parameter)
        {
            if (operation == null)
            {
                this.Log().Info($"Operation为空");
                return;
            }
            if (operation is T t)
            {
                if (parameter is M m)
                {
                    Action.Invoke(t, m);
                }
                else
                {
                    this.Log().Info($"Parameter类型不为{typeof(M)}");
                }
            }
            else
            {
                this.Log().Info($"Operation类型不为{typeof(T)}");
            }
        }
    }
}
