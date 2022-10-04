namespace Popcron.CommandRunner
{
    public readonly struct InfoCommand : ICommand
    {
        public string Path => "info";

        public Result Run(Context parameters)
        {
            return null;
        }
    }
}