using System;
using System.Collections.Generic;
using System.Text;

namespace Popcron.CommandRunner
{
    [RegisterIntoSingleton]
    public readonly struct TimeCommand : ICommand, ICommandInformation
    {
        ReadOnlySpan<char> IBaseCommand.Path => "time".AsSpan();
        IEnumerable<Type> IBaseCommand.Parameters => Array.Empty<Type>();

        void ICommandInformation.Append(StringBuilder stringBuilder)
        {
            stringBuilder.Append("The current time.");
        }

        object ICommand.Run(ExecutionInput context)
        {
            return DateTime.Now;
        }
    }
}