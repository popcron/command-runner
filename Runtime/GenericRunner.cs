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

        public void Run(string text)
        {
            if (parser.TryParse(text, out CommandInput path))
            {
                IBaseCommand prefab = library.GetPrefab(path);
                Context parameters = new Context(library);
                if (prefab is ICommand command)
                {
                    command.Run(parameters);
                }
            }
        }
    }
}