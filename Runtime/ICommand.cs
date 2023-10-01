#nullable enable
namespace Popcron.CommandRunner
{
    public interface ICommand : IBaseCommand
    {
        object? Run(ExecutionInput input);
    }
}