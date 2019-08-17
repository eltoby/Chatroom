namespace StockBot.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CommandParserTests
    {
        private CommandParser sut;

        [TestInitialize]
        public void Setup() => this.sut = new CommandParser();
        [TestMethod]
        public void MessagesStartingWithoutSlashAreNotCommands()
        {
            var cmd = this.sut.Parse("Hello");
            Assert.IsFalse(cmd.IsCommand);            
        }

        [TestMethod]
        public void MessageStartingWithSlashIsCommand()
        {
            var cmd = this.sut.Parse("/stock=code");
            Assert.IsTrue(cmd.IsCommand);
        }

        [TestMethod]
        public void CommandsMustContainEqual()
        {
            var cmd = this.sut.Parse("/stock:code");
            Assert.IsFalse(cmd.IsCommand);
        }

        [TestMethod]
        public void CommandsMustContainKeyAndValue()
        {
            var cmd = this.sut.Parse("/=");
            Assert.IsFalse(cmd.IsCommand);
        }

        [TestMethod]
        public void CommandKeyParsedCorrectly()
        {
            var cmd = this.sut.Parse("/stock=code");
            Assert.AreEqual("stock", cmd.Key);
        }

        [TestMethod]
        public void CommandValueParsedCorrectly()
        {
            var cmd = this.sut.Parse("/stock=code");
            Assert.AreEqual("code", cmd.Value);
        }
    }
}
