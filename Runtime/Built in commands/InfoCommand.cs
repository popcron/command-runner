namespace Popcron.CommandRunner
{
    [AutoRegister]
    public struct InfoCommand : ICommand
    {
        public string Path => "info";

        public Result Run(Context parameters)
        {
            return null;
        }
    }
}