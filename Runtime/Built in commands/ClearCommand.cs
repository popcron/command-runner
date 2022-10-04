using UnityEngine;

namespace Popcron.CommandRunner
{
    public readonly struct ClearCommand : ICommand, IDescription
    {
        public string Path => "clear";
        public string Description => "Clears the debug console.";

        public Result Run(Context parameters)
        {
            Debug.ClearDeveloperConsole();
            return null;
        }
    }
}