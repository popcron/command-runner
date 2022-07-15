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

        public static ReadOnlyCollection<IBaseCommand> FindAllCommands()
        {
            List<IBaseCommand> commands = new List<IBaseCommand>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                AddCommands(commands, assembly);
            }

            return new ReadOnlyCollection<IBaseCommand>(commands);
        }

        [Command("yo")]
        public static void Yo()
        {
            Debug.Log("aye");
        }

        private static void AddCommands(List<IBaseCommand> commands, Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                AutoRegisterAttribute attribute = type.GetCustomAttribute<AutoRegisterAttribute>();
                if (attribute != null)
                {
                    IBaseCommand command = Activator.CreateInstance(type) as IBaseCommand;
                    if (command != null)
                    {
                        commands.Add(command);
                    }
                    else
                    {
                        Debug.LogError($"Cannot auto register type {type} as a command because it doesnt implement {nameof(IBaseCommand)}");
                    }
                }
                else
                {
                    MethodInfo[] methods = type.GetMethods(Flags);
                    foreach (MethodInfo method in methods)
                    {
                        CommandAttribute commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                        if (commandAttribute != null)
                        {
                            commands.Add(commandAttribute);
                        }
                    }
                }
            }
        }
    }
}