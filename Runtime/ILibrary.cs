#nullable enable
using System;
using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public interface ILibrary
    {
        IReadOnlyCollection<IBaseCommand> Commands { get; }

        bool TryGetCommand(ReadOnlySpan<char> path, InputParameters input, out IBaseCommand command);
        void Add(IBaseCommand prefab);
        void Clear();
    }

    public static class ILibraryFunctions
    {
        public static void AddCommand(this ILibrary library, IBaseCommand command)
        {
            library.Add(command);
        }

        public static void AddCommands<T>(this ILibrary library, IEnumerable<IBaseCommand> prefabs)
        {
            foreach (IBaseCommand prefab in prefabs)
            {
                AddCommand(library, prefab);
            }
        }

        public static IBaseCommand? GetCommand(this ILibrary library, ReadOnlySpan<char> path, InputParameters parameters)
        {
            if (library.TryGetCommand(path, parameters, out IBaseCommand command))
            {
                return command;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the name of the command type <typeparamref name="T"/> implements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="library"></param>
        /// <returns><c>default</c> when no instance of the type exists.</returns>
        public static ReadOnlySpan<char> GetCommandName<T>(this ILibrary library)
        {
            foreach (IBaseCommand command in library.Commands)
            {
                if (command is T)
                {
                    return command.Path;
                }
            }

            return ReadOnlySpan<char>.Empty;
        }
    }
}