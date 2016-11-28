using Exercise.UI.Model.Contract;
using FluentAssertions;
using NUnit.Framework;
using Exercise.UI.ViewModel;
using FizzWare.NBuilder;
using Moq;

namespace Exercise.Test
{
    [TestFixture]
    public class StockTest : TestWithSubject<Stock>
    {
        private static readonly StockType[] StocksTypes = EnumHelper.GetValues<StockType>();
        private Mock<IStockConfiguration> _stockConfig;

        public override Stock CreateSubject()
        {
            _stockConfig = new Mock<IStockConfiguration>();
            _stockConfig.Setup(e => e.GetTolerance(It.IsAny<StockType>()))
                .Returns<StockType>(e => e == StockType.Bond ? 100000 : 200000);
            _stockConfig.Setup(e => e.GetTransactionCost(It.IsAny<StockType>()))
               .Returns<StockType>(e => e == StockType.Bond ? 0.02M : 0.005M);
            return new Stock(_stockConfig.Object);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void Create_ForStockOfGivenType_ShouldCalculateMarketValue(StockType stockType)
        {
            Subject.StockType = stockType;
            Subject.Price = 1.2M;
            Subject.Quantity = 3;

            Subject.MarektValue.Should().Be(3.6M);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void UpdateQuantity_StockOfGivenType_ShouldRecalculateMarketValue(StockType stockType)
        {
            Subject.StockType = stockType;
            Subject.Price = 1.2M;
            Subject.Quantity = 3;
            Subject.Quantity = 4;
            
            Subject.MarektValue.Should().Be(4.8M);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void UpdatePrice_ForStockOfGivenType_ShouldRecalculateMarketValue(StockType stockType)
        {
            Subject.StockType = stockType;
            Subject.Price = 1.2M;
            Subject.Quantity = 3;
            Subject.Price = 2.3M;

            Subject.MarektValue.Should().Be(6.9M);
        }

        [Test]
        public void UpdateMarketValue_ForLowTransactionCostStockType_CalculatedTransactionCost()
        {
            Subject.StockType = StockType.Equity;
            Subject.Price = 2.3M;
            Subject.Quantity = 3;

            Subject.MarektValue.Should().Be(6.9M);
            Subject.TransactionCost.Should().Be(6.9M * 0.005M);
        }

        [Test]
        public void UpdateMarketValue_ForHighTransactionCostStockType_CalculatedTransactionCost()
        {
            Subject.StockType = StockType.Bond;
            Subject.Price = 2.3M;
            Subject.Quantity = 3;

            Subject.MarektValue.Should().Be(6.9M);
            Subject.TransactionCost.Should().Be(6.9M * 0.02M);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void UpdateInTolerance_ForPositiveMarektValue_ShouldBeTrue(StockType stockType)
        {
            Subject.StockType = stockType;
            Subject.Price = 1.2M;
            Subject.Quantity = 3;

            Subject.IsInTolerance.Should().Be(true);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void UpdateInTolerance_ForNegativeMarektValue_ShouldBeFalse(StockType stockType)
        {
            Subject.StockType = stockType;
            Subject.Price = -1.2M;
            Subject.Quantity = 3;

            Subject.IsInTolerance.Should().Be(false);
        }

        [TestCase(StockType.Bond, 0, true)]
        [TestCase(StockType.Bond, 100, true)]
        [TestCase(StockType.Bond, 100000, true)]
        [TestCase(StockType.Bond, 100001, false)]
        [TestCase(StockType.Bond, 200000, false)]
        [TestCase(StockType.Bond, 200001, false)]
        [TestCase(StockType.Equity, 0, true)]
        [TestCase(StockType.Equity, 100, true)]
        [TestCase(StockType.Equity, 100000, true)]
        [TestCase(StockType.Equity, 100001, true)]
        [TestCase(StockType.Equity, 200000, true)]
        [TestCase(StockType.Equity, 200001, false)]
        public void UpdateInTolerance_GivenStockTypeAndTransactionCost_ShouldHasExpectedValue(StockType stockType, decimal transportCost, bool expectedFlagValue)
        {
            Subject.StockType = stockType;
            Subject.TransactionCost = transportCost;

            Subject.IsInTolerance.Should().Be(expectedFlagValue);
        }
    }
}