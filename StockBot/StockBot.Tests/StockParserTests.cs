namespace StockBot.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StockParserTests
    {
        [TestMethod]
        public void ParseStockResponse()
        {
            var sut = new StockParser();
            var response = "Symbol,Date,Time,Open,High,Low,Close,Volume\r\nAAPL.US,2019 - 08 - 16,22:00:01,204.28,207.16,203.84,206.5,28813624\r\n";
            var stock = sut.Parse(response);
            Assert.AreEqual("AAPL.US", stock.Symbol);
            Assert.AreEqual("206.5", stock.Close);
        }
    }
}
