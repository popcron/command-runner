using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public readonly struct ListAllCommands : ICommand, IDescription
    {
        string IBaseCommand.Path => "ls commands";
        string IDescription.Description => "Prints a list of all commands available";

        public Result Run(Context parameters)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IBaseCommand command in parameters.Library.Prefabs)
            {
                command.AppendBasicInformation(sb);
                sb.AppendLine();
            }

            Debug.Log(sb.ToString());
            return null;
        }
    }
}