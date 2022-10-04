using System.Threading.Tasks;

namespace Popcron.CommandRunner
{
    public class CommandRunner : ICommandRunner
    {
        private ILibrary library;
        private IParser parser;

        public ILibrary Library => library;

        public static CommandRunner Singleton { get; } = new CommandRunner(Popcron.CommandRunner.Library.Singleton, new ClassicParser());

        public CommandRunner(ILibrary library, IParser parser)
        {
            this.library = library;
            this.parser = parser;
        }

        private Result Run(ICommand command)
        {
            Context parameters = new Context(library);
            Result result = new Result();
            result.Set(command.Run(parameters));
            return new Result(result.Value, result.Logs);
        }

        public Result Run(string text)
        {
            CommandInput path = parser.Parse(text);
            if (!path.IsEmpty)
            {
                IBaseCommand prefab = library.GetPrefab(path);
                if (prefab is ICommand command)
                {
                    return Run(command);
                }
            }

            return null;
        }

        public async Task<Result> RunAsync(string text)
        {
            CommandInput path = parser.Parse(text);
            if (!path.IsEmpty)
            {
                IBaseCommand prefab = library.GetPrefab(path);
                if (prefab is IAsyncCommand asyncCommand)
                {
                    Context parameters = new Context(library);
                    Result result = new Result();
                    result.Set(await asyncCommand.RunAsync(parameters));
                    return new Result(result.Value, result.Logs);
                }
                else if (prefab is ICommand command)
                {
                    return Run(command);
                }
            }

            return null;
        }
    }
}