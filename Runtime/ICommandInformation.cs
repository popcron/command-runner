using System.Text;

namespace Popcron.CommandRunner
{
    public interface ICommandInformation
    {
        void Append(StringBuilder stringBuilder);
    }
}