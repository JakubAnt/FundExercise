using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exercise.Test
{
    [TestFixture]
    public class StocksTest : TestWithSubject<Stocks>
    {
        private static readonly StockType[] StocksTypes = EnumHelper.GetValues<StockType>();
        private Mock<IStockConfiguration> _stockConfig;

        [SetUp]
        public void Setup()
        {
            _stockConfig = new Mock<IStockConfiguration>();
        }

        public override Stocks CreateSubject()
        {
            var mock = new Mock<IStockService>();
            return new Stocks(mock.Object);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void StocksAdded_GenerateName_NameWithNextNumberOfGivenStockType(StockType stockType)
        {
            var stock = new Stock(_stockConfig.Object) { StockType = stockType };

            Subject.Add(stock);

            stock.Name.Should().Be(stockType + "1");
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void NextStocksAdded_GenerateName_NameWithNextNumberOfGivenStockType(StockType stockType)
        {
            var stock1 = new Stock(_stockConfig.Object) { StockType = stockType };
            var stock2 = new Stock(_stockConfig.Object) { StockType = stockType };

            Subject.Add(stock1);
            Subject.Add(stock2);

            stock1.Name.Should().Be(stockType + "1");
            stock2.Name.Should().Be(stockType + "2");
        }

        [Test]
        public void StocksAdded_ForDifferentType_StockNamedWithOwnNumber()
        {
            var stock1 = new Stock(_stockConfig.Object) { StockType = StockType.Bond };
            var stock2 = new Stock(_stockConfig.Object) { StockType = StockType.Equity };

            Subject.Add(stock1);
            Subject.Add(stock2);

            stock1.Name.Should().Be("Bond1");
            stock2.Name.Should().Be("Equity1");
        }
    }
}
