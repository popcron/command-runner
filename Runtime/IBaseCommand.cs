#nullable enable
using System;
using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public interface IBaseCommand
    {
        ReadOnlySpan<char> Path { get; }
        IEnumerable<Type> Parameters { get; }
    }

    public static class ICommandFunctions
    {
        public static object? Run<T>(this T command, ExecutionInput input) where T : IBaseCommand
        {
            if (command is ICommand c)
            {
                return c.Run(input);
            }
            else if (command is IAsyncCommand asyncCommand)
            {
                return asyncCommand.RunAsync(input, default);
            }
            else throw new Exception($"Command type '{command}' is not an executable command");
        }
    }
}