using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [AutoRegister]
    public struct HelpCommand : ICommand
    {
        public string Path => "help";

        public void Run(Context parameters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ICommand command in parameters.Library.Prefabs)
            {
                sb.Append(command.Path);
                sb.Append(" = ");
                sb.Append(command.Path);
            }

            Debug.Log(sb.ToString());
        }
    }
}