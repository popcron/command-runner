#nullable enable
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Popcron.CommandRunner
{
    public interface IAsyncCommand : IBaseCommand
    {
        UniTask<object?> RunAsync(ExecutionInput input, CancellationToken cancellationToken = default);
    }
}