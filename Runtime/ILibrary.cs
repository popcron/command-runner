using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public interface ILibrary
    {
        IEnumerable<IBaseCommand> Prefabs { get; }

        IBaseCommand GetPrefab(CommandInput path);
        void Add(IBaseCommand prefab);
        void Clear();
    }
}