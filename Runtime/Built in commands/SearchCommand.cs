using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [AutoRegister]
    public struct SearchCommand : ICommand<string>, IDescription
    {
        string IBaseCommand.Path => "search";
        string IDescription.Description => "Searches for commands";

        Result ICommand<string>.Run(Context parameters, string search)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IBaseCommand command in parameters.Library.Search(search))
            {
                command.AppendBasicInformation(sb);
                sb.AppendLine();
            }

            Debug.Log(sb.ToString());
            return null;
        }
    }
}