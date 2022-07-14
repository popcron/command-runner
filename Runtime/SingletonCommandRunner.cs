using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public static class SingletonCommandRunner
    {
        private static CommandRunner runner;

        public static CommandRunner Instance
        {
            get
            {
                if (runner is null)
                {
                    Initialize();
                }

                return runner;
            }
        }

        private static void Initialize()
        {
            IEnumerable<ICommand> commands = CommandFinder.FindAllCommands();
            ILibrary library = new Library(commands);
            IParser parser = new ClassicParser();
            runner = new CommandRunner(library, parser);
        }
    }
}