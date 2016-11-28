using Exercise.UI.Infrastructure;
using Exercise.UI.Model.Contract;

namespace Exercise.UI.ViewModel.Factories
{
    public class FundViewModelFactory: IFactory<FundViewModel>
    {
        private readonly IStockService _service;
        private readonly IFactory<Stock> _stockFactory;

        public FundViewModelFactory(IStockService service, IFactory<Stock> stockFactory)
        {
            _service = service;
            _stockFactory = stockFactory;
        }
        public FundViewModel Create()
        {
            return new FundViewModel(_service, _stockFactory);
        }
    }
}