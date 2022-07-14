using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public static class Extensions
    {
        public static bool TryCreate(this ILibrary library, CommandInput input, out ICommand command)
        {
            foreach (ICommand prefab in library.Prefabs)
            {
                if (input.Equals(prefab.Path))
                {
                    return library.TryCreate(input, out command);
                }
            }

            command = null;
            return false;
        }

        public static void AddRange<T>(this ILibrary library, IEnumerable<ICommand> prefabs)
        {
            foreach (ICommand prefab in prefabs)
            {
                library.Add(prefab);
            }
        }
    }
}