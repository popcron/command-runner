using System;
using System.Collections.Generic;
using System.Text;

namespace Popcron.CommandRunner
{
    [RegisterIntoSingleton]
    public readonly struct ListAllCommands : ICommand, ICommandInformation
    {
        private static readonly StringBuilder sb = new StringBuilder();

        ReadOnlySpan<char> IBaseCommand.Path => "ls commands";
        IEnumerable<Type> IBaseCommand.Parameters => Array.Empty<Type>();

        void ICommandInformation.Append(StringBuilder stringBuilder)
        {
            stringBuilder.Append("Lists all commands available");
        }

        object ICommand.Run(ExecutionInput context)
        {
            sb.Clear();
            foreach (IBaseCommand command in context.library.Commands)
            {
                sb.Append(command.Path);
                sb.Append(" = ");
                if (command is ICommandInformation information)
                {
                    information.Append(sb);
                }
                else
                {
                    sb.Append(command.GetType().FullName);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}