#nullable enable
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [RegisterIntoSingleton]
    public readonly struct SearchCommand : IAsyncCommand, ICommandInformation
    {
        ReadOnlySpan<char> IBaseCommand.Path => "search";
        IEnumerable<Type> IBaseCommand.Parameters
        {
            get
            {
                yield return typeof(string);
            }
        }

        void ICommandInformation.Append(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("Searches for commands");
        }

        async UniTask<object?> IAsyncCommand.RunAsync(ExecutionInput input, CancellationToken cancellationToken)
        {
            string? search = input.parameters[0]?.ToString();
            if (search != null)
            {
                return await Search(search, input.library);
            }
            else return null;
        }

        public IEnumerable<IBaseCommand> Search(ReadOnlySpan<char> search, ILibrary library)
        {
            HashSet<IBaseCommand> searchResult = new HashSet<IBaseCommand>();
            foreach (IBaseCommand command in library.Commands)
            {
                if (command.Path.Contains(search, StringComparison.OrdinalIgnoreCase))
                {
                    searchResult.Add(command);
                }
            }

            return searchResult;
        }

        public async UniTask<string> Search(string search, ILibrary library)
        {
            const double MaxBlockTime = 0.1;
            DateTime now = DateTime.Now;

            StringBuilder sb = new StringBuilder();
            foreach (IBaseCommand command in Search(search.AsSpan(), library))
            {
                sb.Append(command.Path);
                sb.Append(" = ");
                command.AppendBasicInformation(sb);
                sb.AppendLine();

                double elapsed = (DateTime.Now - now).TotalSeconds;
                if (elapsed > MaxBlockTime)
                {
                    now = DateTime.Now;
                    await UniTask.Yield();
                }
            }

            return sb.ToString();
        }
    }
}