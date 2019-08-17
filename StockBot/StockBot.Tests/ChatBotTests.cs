namespace StockBot.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ChatBotTests
    {
        private ChatBot sut;
        private ICommandParser commandParser;
        private IValidCommandsCatalog validCommandsCatalog;

        [TestInitialize]
        public void Setup()
        {
            this.commandParser = Mock.Of<ICommandParser>();
            this.validCommandsCatalog = Mock.Of<IValidCommandsCatalog>();

            Mock.Get(this.validCommandsCatalog).Setup(x => x.GetValidCommands()).Returns(new[] { "stock" });            
            this.sut = new ChatBot(this.commandParser, this.validCommandsCatalog);
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

        [TestMethod]
        public void ChatBotWarnsOnInvalidCommand()
        {
            var cmd = new BotCommand { IsCommand = true, Key = "invalid" };
            Mock.Get(this.commandParser).Setup(x => x.Parse("/invalid=code")).Returns(cmd);
            var result = this.sut.Process("/invalid=code");
            Assert.AreEqual("Invalid command. Please use a valid command (Valid commands: stock)", result);
        }
    }
}
