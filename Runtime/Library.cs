using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public class Library : ILibrary
    {
        private Dictionary<CommandInput, IBaseCommand> prefabs = new Dictionary<CommandInput, IBaseCommand>();

        public IEnumerable<IBaseCommand> Prefabs
        {
            get
            {
                foreach (var prefab in prefabs)
                {
                    yield return prefab.Value;
                }
            }
        }

        public Library()
        {

        }

        public Library(IEnumerable<IBaseCommand> prefabs)
        {
            foreach (IBaseCommand prefab in prefabs)
            {
                Add(prefab);
            }
        }

        public void Clear()
        {
            prefabs.Clear();
        }

        public IBaseCommand GetPrefab(CommandInput input)
        {
            foreach (KeyValuePair<CommandInput, IBaseCommand> pair in prefabs)
            {
                CommandInput path = pair.Key;
                IBaseCommand prefab = pair.Value;
                int parametersGiven = input.Count - path.Count;
                int parametersExpected = prefab.GetParameterCount();
                if (parametersGiven == parametersExpected)
                {
                    //return prefab;
                }

                if (path.Equals(input))
                {
                    return prefab;
                }
            }

            return null;
        }

        public void Add(IBaseCommand prefab)
        {
            foreach (KeyValuePair<CommandInput, IBaseCommand> pair in prefabs)
            {
                IBaseCommand existingPrefab = pair.Value;
                if (existingPrefab.Path == prefab.Path && existingPrefab.GetType() == prefab.GetType())
                {
                    return;
                }
            }

            CommandInput path = new CommandInput(prefab.Path);
            prefabs[path] = prefab;
        }
    }
}