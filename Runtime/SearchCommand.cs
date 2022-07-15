using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [AutoRegister]
    public struct SearchCommand : ICommand<string>, IDescription
    {
        string IBaseCommand.Path => "search";
        string IDescription.Description => "Searches for commands";

        public void Run(Context parameters, string search)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IBaseCommand command in parameters.Library.Prefabs)
            {
                string commandPath = command.Path;
                string typeName = command.GetType().FullName;
                string description = (command as IDescription)?.Description;
                if (Compare(commandPath, search) || Compare(typeName, search) || Compare(description, search))
                {
                    command.AppendBasicInformation(sb);
                    sb.AppendLine();
                }
            }

            Debug.Log(sb.ToString());
        }

        private bool Compare(string a, string b)
        {
            if (a.Contains(b))
            {
                return true;
            }
            else if (b.Contains(a))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}