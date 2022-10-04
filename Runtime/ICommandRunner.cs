using System.Threading.Tasks;

namespace Popcron.CommandRunner
{
    public interface ICommandRunner
    {
        /// <summary>
        /// Returns an object that contains the result of running the command.
        /// Can be null.
        /// </summary>
        Result Run(string text);

        /// <summary>
        /// Returns an object that contains the result of running the command.
        /// Can be null.
        /// </summary>
        Task<Result> RunAsync(string text);
    }
}