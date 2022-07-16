namespace Popcron.CommandRunner
{
    public class Context
    {
        public ILibrary Library { get; }

        public Context(ILibrary library)
        {
            Library = library;
        }
    }
}