using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public static class CommandFinder
    {
        public const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public static ReadOnlyCollection<ICommand> FindAllCommands()
        {
            List<ICommand> commands = new List<ICommand>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                AddCommands(commands, assembly);
            }

            return new ReadOnlyCollection<ICommand>(commands);
        }

        private static void AddCommands(List<ICommand> commands, Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                AutoRegisterAttribute attribute = type.GetCustomAttribute<AutoRegisterAttribute>();
                if (attribute != null)
                {
                    ICommand command = Activator.CreateInstance(type) as ICommand;
                    if (command != null)
                    {
                        SingletonCommandRunner.Instance.Library.Add(command);
                    }
                    else
                    {
                        Debug.LogError($"Cannot auto register type {type} as a command because it doesnt implement {nameof(ICommand)}");
                    }
                }
                else
                {
                    MethodInfo[] methods = type.GetMethods(Flags);
                }
            }
        }
    }
}