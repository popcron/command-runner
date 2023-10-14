using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [RegisterIntoSingleton]
    public readonly struct ClearCommand : ICommand, ICommandInformation
    {
        ReadOnlySpan<char> IBaseCommand.Path => "clear".AsSpan();
        IEnumerable<Type> IBaseCommand.Parameters => Array.Empty<Type>();

        void ICommandInformation.Append(StringBuilder stringBuilder)
        {
            stringBuilder.Append("Clears the console.");
        }

        object ICommand.Run(ExecutionInput context)
        {
            Debug.ClearDeveloperConsole();
            return null;
        }
    }
}