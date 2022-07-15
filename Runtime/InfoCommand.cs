namespace Popcron.CommandRunner
{
    [AutoRegister]
    public struct InfoCommand : ICommand
    {
        public string Path => "info";

        public void Run(Context parameters)
        {
            
        }
    }
}