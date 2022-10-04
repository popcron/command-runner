namespace Popcron.CommandRunner
{
    public static class ResultExtensions
    {
        public static Result<T> AsResult<T>(this T value)
        {
            return new Result<T>(value);
        }
    }
}