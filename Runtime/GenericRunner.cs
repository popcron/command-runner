using System.Text;

namespace Popcron.CommandRunner
{
    public class CommandRunner : ICommandRunner
    {
        private ILibrary library;
        private IParser parser;

        public ILibrary Library => library;

        public CommandRunner(ILibrary library, IParser parser)
        {
            this.library = library;
            this.parser = parser;
        }

        public Result Run(string text)
        {
            if (parser.TryParse(text, out CommandInput path))
            {
                IBaseCommand prefab = library.GetPrefab(path);
                if (prefab is ICommand command)
                {
                    Context parameters = new Context(library);
                    Result result = new Result();
                    StringBuilder log = result.Set(command.Run(parameters));
                    return new Result(result?.Value, log);
                }
            }

            return null;
        }
    }
}