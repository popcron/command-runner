using NUnit.Framework;

namespace Popcron.CommandRunner
{
    [TestFixture]
    public class CommandTests
    {
        private CommandRunner runner;

        [SetUp]
        public void SetUp()
        {
            ClassicParser parser = new ClassicParser();
            Library library = new Library();
            runner = new CommandRunner(library, parser);
        }

        [Test]
        public void RunCommand()
        {
            string command = "testCommand";
            runner.Library.Add(new MethodCommand(command, DoSomething));

            CommandInput input = (CommandInput)command;
            Assert.IsNotNull(runner.Library.GetPrefab(input));

            runner.Run(command);
        }

        private void DoSomething()
        {

        }
    }

    [TestFixture]
    public class ParsingTests
    {
        private IParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new ClassicParser();
        }

        [Test]
        public void ParseOneWord()
        {
            string text = "apple";
            if (parser.TryParse(text, out CommandInput input))
            {
                Assert.IsTrue(input.Equals(text));
            }
            else
            {
                Assert.Fail($"Parser {parser} couldnt parse {text}");
            }
        }

        [Test]
        public void ParseTwoWords()
        {
            string text = "ls commands";
            if (parser.TryParse(text, out CommandInput input))
            {
                Assert.AreEqual(input.Count, 2);
                Assert.AreEqual(input[0], "ls");
                Assert.AreEqual(input[1], "commands");
            }
            else
            {
                Assert.Fail($"Parser {parser} couldnt parse {text}");
            }
        }

        [Test]
        public void ParseFiveWords()
        {
            string text = "ls commands at bazinga land";
            if (parser.TryParse(text, out CommandInput input))
            {
                Assert.AreEqual(input.Count, 5);
                Assert.AreEqual(input[0], "ls");
                Assert.AreEqual(input[1], "commands");
                Assert.AreEqual(input[2], "at");
                Assert.AreEqual(input[3], "bazinga");
                Assert.AreEqual(input[4], "land");
            }
            else
            {
                Assert.Fail($"Parser {parser} couldnt parse {text}");
            }
        }
    }
}