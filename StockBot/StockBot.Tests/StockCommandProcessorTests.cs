namespace StockBot.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class StockCommandProcessorTests
    {
        private StockCommandProcessor sut;
        private IStockService stockService;

        [TestInitialize]
        public void Setup()
        {
            this.stockService = Mock.Of<IStockService>();
            this.sut = new StockCommandProcessor(this.stockService);
        }

        [TestMethod]
        public void NullStockRespondsAsInvalidStock()
        {
            Mock.Get(this.stockService).Setup(x => x.GetStock("code")).Returns<Stock>(null);
            this.sut.SetCommand(new BotCommand() { Value = "code" });

            var result = this.sut.Process();

            Assert.AreEqual("Invalid stock: code", result);
        }

        [TestMethod]
        public void InformOnNotAvailableStock()
        {
            var stock = new Stock()
            {
                Symbol = "CODE",
                Close = "N/D"
            };

            Mock.Get(this.stockService).Setup(x => x.GetStock("code")).Returns(stock);
            this.sut.SetCommand(new BotCommand() { Value = "code" });

            var result = this.sut.Process();

            Assert.AreEqual("CODE quote is not available", result);
        }

        [TestMethod]
        public void ReturnQuote()
        {
            var stock = new Stock()
            {
                Symbol = "CODE",
                Close = "93.62"
            };

            Mock.Get(this.stockService).Setup(x => x.GetStock("code")).Returns(stock);
            this.sut.SetCommand(new BotCommand() { Value = "code" });

            var result = this.sut.Process();

            Assert.AreEqual("CODE quote is $93.62 per share", result);
        }
    }
}
