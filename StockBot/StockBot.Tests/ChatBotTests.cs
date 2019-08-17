namespace StockBot.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ChatBotTests
    {
        private ChatBot sut;
        private ICommandParser commandParser;

        [TestInitialize]
        public void Setup()
        {
            this.commandParser = Mock.Of<ICommandParser>();
            this.sut = new ChatBot(this.commandParser);
        }

        [TestMethod]
        public void ChatBotProcessesOnlyCommands()
        {
            var cmd = new BotCommand() { IsCommand = false };
            Mock.Get(this.commandParser).Setup(x => x.Parse("Hello")).Returns(cmd);
            var result = this.sut.Process("Hello");
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ChatBotRespondsOnCommands()
        {
            var cmd = new BotCommand() { IsCommand = true};
            Mock.Get(this.commandParser).Setup(x => x.Parse("/stock=code")).Returns(cmd);
            var result = this.sut.Process("/stock=code");
            Assert.AreNotEqual(string.Empty, result);
        }
    }
}
