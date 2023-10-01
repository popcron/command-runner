using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    [RegisterIntoSingleton]
    public readonly struct InfoCommand : ICommand, ICommandInformation
    {
        private static readonly StringBuilder builder = new StringBuilder();

        ReadOnlySpan<char> IBaseCommand.Path => "info";
        IEnumerable<Type> IBaseCommand.Parameters => Array.Empty<Type>();

        void ICommandInformation.Append(StringBuilder stringBuilder)
        {
            stringBuilder.Append("Information about the environment running in");
        }

        object ICommand.Run(ExecutionInput input)
        {
            builder.Clear();
            builder.Append("Platform: ");
            builder.Append(Application.platform);
            builder.AppendLine();

            builder.Append("Unity Version: ");
            builder.Append(Application.unityVersion);
            builder.AppendLine();

            builder.Append("Version: ");
            builder.Append(Application.version);
            builder.AppendLine();

            builder.Append("Company Name: ");
            builder.Append(Application.companyName);
            builder.AppendLine();

            builder.Append("Product Name: ");
            builder.Append(Application.productName);
            builder.AppendLine();

            builder.Append("Data Path: ");
            builder.Append(Application.dataPath);
            builder.AppendLine();

            builder.Append("Persistent Data Path: ");
            builder.Append(Application.persistentDataPath);

            return builder.ToString();
        }
    }
}