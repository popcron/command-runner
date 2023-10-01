#nullable enable
using Cysharp.Threading.Tasks;

namespace Popcron.CommandRunner
{
    public interface ICommandRunner
    {
        UniTask<ExecutionResult> RunAsync<T>(T command, InputParameters parameters) where T : IBaseCommand;

        /// <summary>
        /// Returns an object that contains the result of running the command.
        /// Can be null.
        /// </summary>
        UniTask<ExecutionResult> FindAndRunAsync(string text);
    }

    public static class ICommandRunnerFunctions
    {
        public static UniTask<ExecutionResult> FindAndRunAsync(this ICommandRunner runner, string text)
        {
            return runner.FindAndRunAsync(text);
        }

        public static UniTask<ExecutionResult> Run<T>(this ICommandRunner runner, T command) where T : IBaseCommand
        {
            return runner.RunAsync(command, default);
        }

        public static UniTask<ExecutionResult> Run<T, P1>(this ICommandRunner runner, T command, P1 p1) where T : IBaseCommand
        {
            return runner.RunAsync(command, InputParameters.Create(p1));
        }

        public static UniTask<ExecutionResult> Run<T, P1, P2>(this ICommandRunner runner, T command, P1 p1, P2 p2) where T : IBaseCommand
        {
            return runner.RunAsync(command, InputParameters.Create(p1, p2));
        }

        public static UniTask<ExecutionResult> Run<T, P1, P2, P3>(this ICommandRunner runner, T command, P1 p1, P2 p2, P3 p3) where T : IBaseCommand
        {
            return runner.RunAsync(command, InputParameters.Create(p1, p2, p3));
        }

        public static UniTask<ExecutionResult> Run<T, P1, P2, P3, P4>(this ICommandRunner runner, T command, P1 p1, P2 p2, P3 p3, P4 p4) where T : IBaseCommand
        {
            return runner.RunAsync(command, InputParameters.Create(p1, p2, p3, p4));
        }
    }
}