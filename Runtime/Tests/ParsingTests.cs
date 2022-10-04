using NUnit.Framework;

namespace Popcron.CommandRunner
{
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
            CommandInput input = parser.Parse(text);
            Assert.IsTrue(input.Equals(text));
        }

        [Test]
        public void ParseTwoWords()
        {
            string text = "ls commands";
            CommandInput input = parser.Parse(text);
            Assert.AreEqual(input.Count, 2);
            Assert.AreEqual(input[0], "ls");
            Assert.AreEqual(input[1], "commands");
        }

        [Test]
        public void ParseFiveWords()
        {
            string text = "ls commands at bazinga land";
            CommandInput input = parser.Parse(text);
            Assert.AreEqual(input.Count, 5);
            Assert.AreEqual(input[0], "ls");
            Assert.AreEqual(input[1], "commands");
            Assert.AreEqual(input[2], "at");
            Assert.AreEqual(input[3], "bazinga");
            Assert.AreEqual(input[4], "land");
        }
    }
}