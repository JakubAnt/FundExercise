using System.Linq;
using Castle.Core.Internal;
using Exercise.UI.Infrastructure;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exercise.Test
{
    [TestFixture]
    public class StockSummariesTest : TestWithSubject<StockSummaries>
    {
        private static readonly StockType[] StocksTypes = EnumHelper.GetValues<StockType>();
        private Mock<IStockConfiguration> _stockConfig;

        public Stocks StocksToSum;
        public override StockSummaries CreateSubject()
        {
            var mock = new Mock<IStockService>();
            StocksToSum = new Stocks(mock.Object);
            return new StockSummaries(StocksToSum);
        }

        [SetUp]
        public void Setup()
        {
            _stockConfig = new Mock<IStockConfiguration>();
        }

        [Test]
        public void Create_Empty_ShouldHaveZeroInTotals()
        {
            Subject.Should()
                .HaveCount(StocksTypes.Length + 1, "summary per stock types and one additional for all data").And
                .OnlyContain(s => s.TotalMarektValue == 0).And
                .OnlyContain(s => s.TotalNumber == 0).And
                .OnlyContain(s => s.TotalStockWeight == 0);
        }

        [Test]
        public void InitialStock_EmptyStock_ShouldHaveZeroTotalMarektValue()
        {
            var stock = new Stock(_stockConfig.Object);
            StocksToSum.Add(stock);

            Subject.Should()
                .OnlyContain(s => s.TotalMarektValue == 0).And
                .Contain(s => s.TotalNumber == 1);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void InitialStock_ForGivenType_ShouldUpdateTotals(StockType stockType)
        {
            var stock = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 1.1M,
                Quantity = 2
            };

            StocksToSum.Add(stock);
           
            var summariesToUpdate = Subject.Where(sum => sum.IsSummaryForStock(stock)).ToArray();
            summariesToUpdate.Should().HaveCount(2, "summary of given types and one additional for all data").And
                .OnlyContain(s => s.TotalMarektValue == 2.2M).And
                .OnlyContain(s => s.TotalNumber == 1).And
                .OnlyContain(s => s.TotalStockWeight == 1);
            Subject.Except(summariesToUpdate)
                .Should().OnlyContain(s => s.TotalMarektValue == 0 && s.TotalNumber == 0 && s.TotalStockWeight == 0);
            stock.StockWeight.Should().Be(1);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void StockUpdated_PriceChanged_ShouldUpdateTotals(StockType stockType)
        {
            var stock = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 1.1M,
                Quantity = 2
            };
            StocksToSum.Add(stock);

            stock.Price = 2.3M;

            var summariesToUpdate = Subject.Where(sum => sum.IsSummaryForStock(stock)).ToArray();
            summariesToUpdate.Should()
                .OnlyContain(s => s.TotalMarektValue == 4.6M).And
                .OnlyContain(s => s.TotalNumber == 1).And
                .OnlyContain(s => s.TotalStockWeight == 1);
            Subject.Except(summariesToUpdate).Should().OnlyContain(s => s.TotalMarektValue == 0 && s.TotalNumber == 0 && s.TotalStockWeight == 0);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void StockUpdated_QuantityChanged_ShouldUpdateTotals(StockType stockType)
        {
            var stock = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 1.1M,
                Quantity = 2
            };
            StocksToSum.Add(stock);

            stock.Quantity = 3;

            var summariesToUpdate = Subject.Where(sum => sum.IsSummaryForStock(stock)).ToArray();
            summariesToUpdate.Should()
                .OnlyContain(s => s.TotalMarektValue == 3.3M).And
                .OnlyContain(s => s.TotalNumber == 1).And
                .OnlyContain(s => s.TotalStockWeight == 1);
            Subject.Except(summariesToUpdate).Should().OnlyContain(s => s.TotalMarektValue == 0 && s.TotalNumber == 0 && s.TotalStockWeight == 0);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void StockAdded_ForNotEmptyFund_ShouldUpdateTotals(StockType stockType)
        {
            var stock1 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 1.1M,
                Quantity = 3
            };
            var stock2 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 3.3M,
                Quantity = 2
            };
            StocksToSum.Add(stock1);
            var summariesToUpdate = Subject.Where(sum => sum.IsSummaryForStock(stock1)).ToArray();
            
            StocksToSum.Add(stock2);
            
            summariesToUpdate.Should()
             .OnlyContain(s => s.TotalMarektValue == 3.3M + 6.6M).And
             .OnlyContain(s => s.TotalNumber == 2).And
             .OnlyContain(s => s.TotalStockWeight == 1);
            Subject.Except(summariesToUpdate).Should().OnlyContain(s => s.TotalMarektValue == 0 && s.TotalNumber == 0 && s.TotalStockWeight == 0);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void StockRemoved_ForNotEmptyFund_ShouldUpdateTotals(StockType stockType)
        {
            var stock1 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 1.1M,
                Quantity = 3
            };
            var stock2 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 3.3M,
                Quantity = 2
            };
            var stock3 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 5.6M,
                Quantity = 7
            };
            StocksToSum.Add(stock1);
            StocksToSum.Add(stock2);
            StocksToSum.Add(stock3);
            var summariesToUpdate = Subject.Where(sum => sum.IsSummaryForStock(stock1)).ToArray();

            StocksToSum.Remove(stock3);

            summariesToUpdate.Should()
             .OnlyContain(s => s.TotalMarektValue == 3.3M + 6.6M).And
             .OnlyContain(s => s.TotalNumber == 2).And
             .OnlyContain(s => s.TotalStockWeight == 1);
            Subject.Except(summariesToUpdate).Should().OnlyContain(s => s.TotalMarektValue == 0 && s.TotalNumber == 0 && s.TotalStockWeight == 0);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void StockAdded_ForNotEmptyFund_ShouldUpdateElementsWeight(StockType stockType)
        {
            var stock1 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 1.1M,
                Quantity = 2
            };
            var stock2 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 3.3M,
                Quantity = 1
            };
            StocksToSum.Add(stock1);
            StocksToSum.Add(stock2);
            
            stock1.StockWeight.Should().Be(0.4M);
            stock2.StockWeight.Should().Be(0.6M);
        }

        [Test, TestCaseSource(nameof(StocksTypes))]
        public void StockRemoved_ForNotEmptyFund_ShouldUpdateElementsWeight(StockType stockType)
        {
            var stock1 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 1.1M,
                Quantity = 2
            };
            var stock2 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 3.3M,
                Quantity = 1
            };
            var stock3 = new Stock(_stockConfig.Object)
            {
                StockType = stockType,
                Price = 5.6M,
                Quantity = 7
            };
            StocksToSum.Add(stock1);
            StocksToSum.Add(stock2);

            StocksToSum.Remove(stock3);

            stock1.StockWeight.Should().Be(0.4M);
            stock2.StockWeight.Should().Be(0.6M);
        }

        [Test]
        public void StocksAdded_ForDifferentStockType_ShouldUpdateTotalWeight()
        {
            var stock1 = new Stock(_stockConfig.Object)
            {
                StockType = StockType.Bond,
                Price = 1.1M,
                Quantity = 2
            };
            var stock2 = new Stock(_stockConfig.Object)
            {
                StockType = StockType.Equity,
                Price = 3.3M,
                Quantity = 1
            };
            StocksToSum.Add(stock1);
            StocksToSum.Add(stock2);

            Subject.Single(sum => sum.IsSummaryForStock(stock1) && !sum.IsSummaryForStock(stock2))
                .TotalStockWeight.Should().Be(stock1.StockWeight).And.NotBe(1);
            Subject.Single(sum => sum.IsSummaryForStock(stock2) && !sum.IsSummaryForStock(stock1))
                .TotalStockWeight.Should().Be(stock2.StockWeight).And.NotBe(1);
            Subject.Single(sum => sum.IsSummaryForStock(stock1) && sum.IsSummaryForStock(stock2))
                .TotalStockWeight.Should().Be(1);
        }

        [TestCase(3)]
        [TestCase(7)]
        [TestCase(10)]
        [TestCase(33)]
        [TestCase(71)]
        [TestCase(100)]
        public void StocksAdded_ForGevenElementCounts_ShouldTypeTotalsSumUpToAllTotal(int elementCount)
        {
            var initialStocks = Builder<Stock>.CreateListOfSize(elementCount).All().WithConstructor(() => new Stock(_stockConfig.Object)).Build();
            var partialSummaies = Subject.Where(t => t.Filter.HasValue).ToList();
            var combinedSummary = Subject.Single(t => t.Filter == null);

            initialStocks.ForEach(s => StocksToSum.Add(s));
           
            partialSummaies.Sum(e => e.TotalMarektValue).Should().Be(combinedSummary.TotalMarektValue).And.BeGreaterThan(0);
            partialSummaies.Sum(e => e.TotalNumber).Should().Be(combinedSummary.TotalNumber).And.Be(elementCount);
            partialSummaies.Should().OnlyContain(e =>
                         e.TotalMarektValue < combinedSummary.TotalMarektValue &&
                         e.TotalNumber < combinedSummary.TotalNumber &&
                         e.TotalStockWeight < combinedSummary.TotalStockWeight);
        }
    }
}
