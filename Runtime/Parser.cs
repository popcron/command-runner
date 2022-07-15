using System.Collections.Generic;

namespace Popcron.CommandRunner
{
    public interface IParser
    {
        bool TryParse(string text, out CommandInput result);
    }

    public interface ICommandRunner
    {
        void Run(string text);
    }

    public interface ILibrary
    {
        IEnumerable<IBaseCommand> Prefabs { get; }

        IBaseCommand GetPrefab(CommandInput path);
        void Add(IBaseCommand prefab);
        void Clear();
    }

    public interface IBaseCommand
    {
        string Path { get; }
    }

    public interface ICommand : IBaseCommand
    {
        void Run(Context context);
    }

    public interface ICommand<T1> : IBaseCommand
    {
        void Run(Context context, T1 p1);
    }

    public interface ICommand<T1, T2> : IBaseCommand
    {
        void Run(Context context, T1 p1, T2 p2);
    }

    public interface ICommand<T1, T2, T3> : IBaseCommand
    {
        void Run(Context context, T1 p1, T2 p2, T3 p3);
    }

    public interface ICommand<T1, T2, T3, T4> : IBaseCommand
    {
        void Run(Context context, T1 p1, T2 p2, T3 p3, T4 p4);
    }

    public interface ICommand<T1, T2, T3, T4, T5> : IBaseCommand
    {
        void Run(Context context, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);
    }

    public interface ICommand<T1, T2, T3, T4, T5, T6> : IBaseCommand
    {
        void Run(Context context, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);
    }
}