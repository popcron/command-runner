#nullable enable

namespace Popcron.CommandRunner
{
    public readonly struct ExecutionInput
    {
        public readonly string? input;
        public readonly ILibrary library;
        public readonly InputParameters parameters;

        public ExecutionInput(string input, ILibrary library, InputParameters parameters)
        {
            this.input = input;
            this.library = library;
            this.parameters = parameters;
        }

        public ExecutionInput(ILibrary library, InputParameters parameters)
        {
            this.input = null;
            this.library = library;
            this.parameters = parameters;
        }
    }
}