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
                Context parameters = new Context(library);
                ICommand prefab = library.GetPrefab(path);
                prefab.Run(parameters);
            }
        }
    }
}