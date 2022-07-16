namespace Popcron.CommandRunner
{
    public interface ICommand : IBaseCommand
    {
        Result Run(Context context);
    }

    public interface ICommand<T1> : IBaseCommand
    {
        Result Run(Context context, T1 p1);
    }

    public interface ICommand<T1, T2> : IBaseCommand
    {
        Result Run(Context context, T1 p1, T2 p2);
    }

    public interface ICommand<T1, T2, T3> : IBaseCommand
    {
        Result Run(Context context, T1 p1, T2 p2, T3 p3);
    }

    public interface ICommand<T1, T2, T3, T4> : IBaseCommand
    {
        Result Run(Context context, T1 p1, T2 p2, T3 p3, T4 p4);
    }

    public interface ICommand<T1, T2, T3, T4, T5> : IBaseCommand
    {
        Result Run(Context context, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5);
    }

    public interface ICommand<T1, T2, T3, T4, T5, T6> : IBaseCommand
    {
        Result Run(Context context, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);
    }
}