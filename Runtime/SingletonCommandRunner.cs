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
                    runner = CreateSingletonRunner();
                }

                return runner;
            }
        }

        private static CommandRunner CreateSingletonRunner()
        {
            IEnumerable<IBaseCommand> commands = CommandFinder.FindAllCommands();
            ILibrary library = new Library(commands);
            IParser parser = new ClassicParser();
            return new CommandRunner(library, parser);
        }
    }
}