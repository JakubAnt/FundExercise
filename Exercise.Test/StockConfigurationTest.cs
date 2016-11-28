using Exercise.UI.Model;
using Exercise.UI.Model.Contract;
using FluentAssertions;
using NUnit.Framework;

namespace Exercise.Test
{
    [TestFixture]
    public class StockConfigurationTest: TestWithSubject<StockConfiguration>
    {

        [TestCase(StockType.Equity, 200 * 1000)]
        [TestCase(StockType.Bond, 100 * 1000)]
        public void GetTolerance_GivenStockType_ExpextedValue(StockType stockType, decimal expectedValue)
        {
            Subject.GetTolerance(stockType).Should().Be(expectedValue);
        }

        [TestCase(StockType.Equity, 0.005)]
        [TestCase(StockType.Bond, 0.02)]
        public void GetTransactionCost_GivenStockType_ExpextedValue(StockType stockType, decimal expectedValue)
        {
            Subject.GetTransactionCost(stockType).Should().Be(expectedValue);
        }
    }
}
