using Exercise.UI.Infrastructure;
using Exercise.UI.Model.Contract;

namespace Exercise.UI.ViewModel.Factories
{
    public class StockFactory : IFactory<Stock>
    {
        private readonly IStockConfiguration _config;

        public StockFactory(IStockConfiguration config)
        {
            _config = config;
        }
        public Stock Create()
        {
            return new Stock(_config);
        }
    }
}