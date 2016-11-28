using FizzWare.NBuilder;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel;
using Moq;

namespace Exercise.Test
{
    [TestFixture]
    public class FundTest : TestWithSubject<Fund>
    {
        private Mock<IStockConfiguration> _stockConfig;
      
        public override Fund CreateSubject()
        {
            var mock = new Mock<IStockService>();
            return new Fund(mock.Object);
        }

        [SetUp]
        public void Setup()
        {
            _stockConfig = new Mock<IStockConfiguration>();
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

            foreach (var stock in initialStocks)
            {
                Subject.Stocks.Add(stock);
            }

            var partialSummaies = Subject.Summaries.Where(t => t.Filter.HasValue).ToList();
            var combinedSummary = Subject.Summaries.Single(t => t.Filter == null);
            partialSummaies.Sum(e => e.TotalMarektValue).Should().Be(combinedSummary.TotalMarektValue).And.BeGreaterThan(0);
            partialSummaies.Sum(e => e.TotalNumber).Should().Be(combinedSummary.TotalNumber).And.Be(elementCount);
            partialSummaies.Should().OnlyContain(e =>
                        e.TotalMarektValue < combinedSummary.TotalMarektValue &&
                        e.TotalNumber < combinedSummary.TotalNumber &&
                        e.TotalStockWeight < combinedSummary.TotalStockWeight);
        }

    }
}
