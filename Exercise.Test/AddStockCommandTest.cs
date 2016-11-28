using System.Runtime.InteropServices;
using Exercise.UI.Infrastructure;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Exercise.Test
{
    [TestFixture]
    public class AddStockCommandTest : TestWithSubject<AddStockCommand>
    {
        public FundViewModel ViewModel;

        public override AddStockCommand CreateSubject()
        {
            var config = new Mock<IStockConfiguration>();
            var factory = new Mock<IFactory<Stock>>();
            factory.Setup(e => e.Create()).Returns(() => new Stock(config.Object));
            var mock = new Mock<IStockService>();
            ViewModel = new FundViewModel(mock.Object, factory.Object);
            return new AddStockCommand(ViewModel.StockToAdd, factory.Object);
        }

        [Test]
        public void CanExecute_EmptyStock_No()
        {
             Subject.CanExecute(ViewModel).Should().BeFalse();
        }

        [Test]
        public void CanExecute_OnlyPrice_No()
        {
            ViewModel.StockToAdd.Price = 1;

            Subject.CanExecute(ViewModel).Should().BeFalse();
        }

        [Test]
        public void CanExecute_OnlyQuantity_No()
        {
            ViewModel.StockToAdd.Quantity = 1;

            Subject.CanExecute(ViewModel).Should().BeFalse();
        }

        [Test]
        public void CanExecute_PriceAndQuantityFilled_Yes()
        {
            ViewModel.StockToAdd.Quantity = 1;
            ViewModel.StockToAdd.Price = 1;

            Subject.CanExecute(ViewModel).Should().BeTrue();
        }

        [Test]
        public void CanExecute_RefreshedOnValueChnaged_OncePerValueUpdate()
        {
            var refreshed = 0;
            Subject.CanExecuteChanged += (s,e) => refreshed++;

            ViewModel.StockToAdd.Quantity = 1;
            ViewModel.StockToAdd.Price = 1;

            refreshed.Should().Be(2);
        }

        [Test]
        public void Execute_ValidStock_AddStockToFund()
        {
            var elementToAdd = ViewModel.StockToAdd;
            ViewModel.StockToAdd.Quantity = 1;
            ViewModel.StockToAdd.Price = 1;

            Subject.Execute(ViewModel);

            ViewModel.Fund.Stocks.Should().NotBeEmpty().And.Contain(elementToAdd);
        }

        [Test]
        public void Execute_ValidStock_ProvideNewStockToFill()
        {
            var elementToAdd = ViewModel.StockToAdd;
            ViewModel.StockToAdd.Quantity = 1;
            ViewModel.StockToAdd.Price = 1;

            Subject.Execute(ViewModel);

            ViewModel.StockToAdd.Should().NotBe(elementToAdd);
        }
    }
}
