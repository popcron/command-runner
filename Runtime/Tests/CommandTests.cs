using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Popcron.CommandRunner
{
    [TestFixture]
    public class CommandTests
    {
        private const string TestCommand = "something";
        private const string TimeCommand = "time";
        private const string AwaitedTimeCommand = "atime";

        private CommandRunner runner;
        private bool ranTestCommand;

        [SetUp]
        public void SetUp()
        {
            ranTestCommand = false;
            ClassicParser parser = new ClassicParser();
            Library library = new Library();
            runner = new CommandRunner(library, parser);

            library.Add(new MethodCommand(TestCommand, TestDoSomething));
            library.Add(new MethodCommand(TimeCommand, ReturnTime));
            library.Add(new MethodCommand(AwaitedTimeCommand, AwaitedReturnTime));
        }

        [Test]
        public void FindTestCommandPrefab()
        {
            Assert.IsNotNull(runner.Library.GetPrefab(TestCommand));
        }

        [Test]
        public void RunTestCommand()
        {
            runner.Run(TestCommand);
            Assert.IsTrue(ranTestCommand);
        }

        [Test]
        public void CheckResultOfCommand()
        {
            Result result = runner.Run(TestCommand);
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task RunAsyncCommand()
        {
            Result result = await runner.RunAsync(TestCommand);
            Assert.IsNotNull(result);
        }

        private void TestDoSomething()
        {
            ranTestCommand = true;
        }

        private Result<DateTime> ReturnTime()
        {
            return DateTime.Now;
        }

        private async Task<Result> AwaitedReturnTime()
        {
            await Task.Delay(1000);
            return DateTime.Now.AsResult();
        }
    }
}