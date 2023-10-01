using System;
using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public static class Extensions
    {
        public static int GetParameterCount(this IBaseCommand command)
        {
            Type type = command.GetType();
            if (type.IsGenericType)
            {
                Debug.Log(type.GetGenericArguments().Length);
            }

            return 0;
        }

        public static void AppendBasicInformation(this IBaseCommand command, StringBuilder stringBuilder)
        {
            stringBuilder.Append(command.Path);
            stringBuilder.Append(" = ");
            if (command is ICommandInformation information)
            {
                information.Append(stringBuilder);
            }
            else
            {
                stringBuilder.Append(command.GetType().FullName);
            }
        }
    }
}