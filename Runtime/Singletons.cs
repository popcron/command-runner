using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public static class Singletons
    {
        private static CommandRunner runner;
        private static Library library;

        public static CommandRunner Runner
        {
            get
            {
                if (runner is null)
                {
                    runner = new CommandRunner(Library, new ClassicParser());
                }

                return runner;
            }
        }

        public static Library Library
        {
            get
            {
                if (library is null)
                {
                    IEnumerable<IBaseCommand> commands = CommandFinder.FindAllCommands();
                    library = new Library();
                }

                return library;
            }
        }
    }
}