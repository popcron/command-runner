using System;
using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    /// <summary>
    /// Where commands are stored and found/fetched from.
    /// </summary>
    public class Library : ILibrary
    {
        public static Library Singleton { get; } = new Library();

        private readonly HashSet<IBaseCommand> commands = new HashSet<IBaseCommand>();

        public IReadOnlyCollection<IBaseCommand> Commands => commands;

        public Library()
        {

        }

        public Library(IEnumerable<IBaseCommand> prefabs)
        {
            foreach (IBaseCommand prefab in prefabs)
            {
                Add(prefab);
            }
        }

        public void Clear()
        {
            commands.Clear();
        }

        public bool TryGetCommand(ReadOnlySpan<char> path, InputParameters parameters, out IBaseCommand command)
        {
            foreach (IBaseCommand existingCommand in commands)
            {
                if (existingCommand.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    command = existingCommand;
                    return true;
                }
            }

            command = default!;
            return false;
        }

        public void Add(IBaseCommand command)
        {
            foreach (IBaseCommand existingCommand in commands)
            {
                if (existingCommand.Path.Equals(command.Path, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"Command with path '{command.Path.ToString()}'({command.GetType()}) already exists");
                }
            }

            commands.Add(command);
        }
    }
}