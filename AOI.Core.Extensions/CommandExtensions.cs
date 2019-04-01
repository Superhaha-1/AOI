using AOI.Core.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;

namespace AOI.Core.Extensions
{
    public static class CommandExtensions
    {
        public static CommandBuilder<T> CreateCommandBuilder<T>(this ContainerBuilder containerBuilder) where T : IOperation, new()
        {
            return new CommandBuilder<T>(containerBuilder);
        }

        public static CommandBuilder<T> Named<T>(this CommandBuilder<T> commandDecorator, string name) where T : IOperation, new()
        {
            commandDecorator.CommandName = name;
            return commandDecorator;
        }

        public static CommandBuilder<T> Setter<T>(this CommandBuilder<T> commandDecorator, string name, Action<T, object> set) where T : IOperation, new()
        {
            commandDecorator.UpdateSetting(name, set);
            return commandDecorator;
        }

        public static void Build<T>(this CommandBuilder<T> commandDecorator) where T : IOperation, new()
        {
            commandDecorator.ContainerBuilder.RegisterBuildCallback(c => c.Resolve<ICommandInitialize>().InitializeCommand(commandDecorator.CommandName, commandDecorator.SetDictionary));
        }
    }

    public sealed class CommandBuilder<T> where T : IOperation, new()
    {
        public CommandBuilder(ContainerBuilder containerBuilder)
        {
            ContainerBuilder = containerBuilder;
            CommandName = typeof(T).Name;
        }

        internal ContainerBuilder ContainerBuilder { get; }

        internal string CommandName { get; set; }

        internal Dictionary<string, Action<T, object>> SetDictionary { get; } = new Dictionary<string, Action<T, object>>();

        internal void UpdateSetting(string name, Action<T, object> set)
        {
            if(SetDictionary.ContainsKey(name))
            {
                SetDictionary[name] = set;
            }
            else
            {
                SetDictionary.Add(name, set);
            }
        }
    }
}
