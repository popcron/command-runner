using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public class Library : ILibrary
    {
        private List<ICommand> prefabs = new List<ICommand>();

        public IEnumerable<ICommand> Prefabs => prefabs;

        public Library(IEnumerable<ICommand> prefabs)
        {
            this.prefabs.AddRange(prefabs);
        }

        public void Clear()
        {
            prefabs.Clear();
        }

        public ICommand GetPrefab(CommandInput path)
        {
            foreach (ICommand prefab in prefabs)
            {
                if (path.Equals(prefab.Path))
                {
                    return prefab;
                }
            }

            return null;
        }

        public void Add(ICommand prefab)
        {
            foreach (ICommand existingPrefab in prefabs)
            {
                if (existingPrefab == prefab)
                {
                    return;
                }
            }

            prefabs.Add(prefab);
        }
    }
}