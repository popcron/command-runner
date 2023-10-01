#nullable enable
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Popcron.CommandRunner
{
    public class AsyncMethodCommand : IAsyncCommand
    {
        private Func<CancellationToken, UniTask<object?>>? methodWithTaskResult;
        private string path;

        public ReadOnlySpan<char> Path => path.AsSpan();
        public IEnumerable<Type> Parameters => Array.Empty<Type>();

        public AsyncMethodCommand(string path, Func<CancellationToken, UniTask<object?>> methodWithTaskResult)
        {
            this.path = path;
            this.methodWithTaskResult = methodWithTaskResult;
        }

        async UniTask<object?> IAsyncCommand.RunAsync(ExecutionInput context, CancellationToken cancellationToken)
        {
            if (methodWithTaskResult != null)
            {
                return await methodWithTaskResult.Invoke(cancellationToken);
            }
            else
            {
                return null;
            }
        }
    }
}