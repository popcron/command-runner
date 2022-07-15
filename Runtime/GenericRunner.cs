using UnityEngine;

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
                if (prefab is ICommand command)
                {
                    Context parameters = new Context(library);
                    command.Run(parameters);
                }
            }
        }
    }
}