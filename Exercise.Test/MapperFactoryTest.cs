using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Exercise.UI.Model;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exercise.Test
{
    [TestFixture]
    public class MapperFactoryTest : TestWithSubject<MapperFactory>
    {
        private Mock<IStockConfiguration> _stockConfig;
        [SetUp]
        public void Setup()
        {
            _stockConfig = new Mock<IStockConfiguration>();
        }

        [Test]
        public void Mapper_ValidStocks_Mapped()
        {
            var exampleStocks = Builder<Stock>.CreateListOfSize(10).All().WithConstructor(() => new Stock(_stockConfig.Object)).Build();

            var mapper = Subject.Create();
            var results = exampleStocks.Select(e => mapper.Map<StockDto>(e));

            results.Should().NotBeEmpty().And.OnlyHaveUniqueItems().And.OnlyContain(e => e.MarektValue != 0 && e.Name != null && e.Price != 0 && e.Quantity != 0 && e.TransactionCost != 0);
        }
    }
}
