using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Popcron.CommandRunner
{
    public static class Extensions
    {
        public static bool TryCreate(this ILibrary library, CommandInput input, out IBaseCommand command)
        {
            foreach (IBaseCommand prefab in library.Prefabs)
            {
                if (input.Equals(prefab.Path))
                {
                    return library.TryCreate(input, out command);
                }
            }

            command = null;
            return false;
        }

        public static IEnumerable<IBaseCommand> Search(this ILibrary library, string search)
        {
            foreach (IBaseCommand prefab in library.Prefabs)
            {
                string commandPath = prefab.Path;
                string typeName = prefab.GetType().FullName;
                string description = (prefab as IDescription)?.Description;
                if (search is null || Compare(commandPath, search) || Compare(typeName, search) || Compare(description, search))
                {
                    yield return prefab;
                }
            }

            bool Compare(string a, string b)
            {
                try
                {
                    if (a.Contains(b))
                    {
                        return true;
                    }
                    else if (b.Contains(a))
                    {
                        return true;
                    }
                }
                catch
                {

                }

                return false;
            }
        }

        public static int GetParameterCount(this IBaseCommand command)
        {
            Type type = command.GetType();
            if (type.IsGenericType)
            {
                Debug.Log(type.GetGenericArguments().Length);
            }

            return 0;
        }

        public static void AddRange<T>(this ILibrary library, IEnumerable<IBaseCommand> prefabs)
        {
            foreach (IBaseCommand prefab in prefabs)
            {
                library.Add(prefab);
            }
        }

        public static void AppendBasicInformation(this IBaseCommand command, StringBuilder stringBuilder)
        {
            stringBuilder.Append(command.Path);
            stringBuilder.Append(" = ");
            if (command is IDescription description)
            {
                stringBuilder.Append(description.Description);
            }
            else
            {
                stringBuilder.Append(command.GetType());
            }
        }
    }
}