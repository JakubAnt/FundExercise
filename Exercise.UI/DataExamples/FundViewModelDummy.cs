using System.Linq;
using Exercise.UI.Infrastructure;
using Exercise.UI.Model;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel;

namespace Exercise.UI.DataExamples
{
    public class FundViewModelDummy : FundViewModel
    {
        public FundViewModelDummy()
            : base(new StockServiceDummy(), new StockFactoryDummy())
        {
            var factory = new StockFactoryDummy();
            StockToAdd = factory.Create();
            StockToAdd.Quantity = 3;
            StockToAdd.Price = 3;

            var stocks = Enumerable.Range(1, 50).Select(i => new { i, Stock = factory.Create()  });

            foreach (var element in stocks)
            {
                element.Stock.StockType = element.i%3 == 0 ? StockType.Bond : StockType.Equity;
                element.Stock.Price = element.i % 31;
                element.Stock.Quantity = element.i % 7 * (element.i % 13 == 0 ? -1 : 1);
                Fund.Stocks.Add(element.Stock);
            }
        }

        public class StockServiceDummy : IStockService
        {
            public void AddStock(Stock stock)
            {
            }

            public void RemoveStock(Stock stock)
            {
            }
        }

        public class StockFactoryDummy : IFactory<Stock>
        {
            public Stock Create()
            {
               return new Stock(new StockConfiguration());
            }
        }
    }
}
