using AutoMapper;
using Exercise.UI.Model.Contract;
using Exercise.UI.Repositories;
using Exercise.UI.ViewModel;

namespace Exercise.UI.Model
{
    public class StockService : IStockService
    {
        private readonly IMapper _mapper;
        private readonly IStockReposiotry _reposiotry;

        public StockService(IMapper mapper, IStockReposiotry reposiotry)
        {
            _mapper = mapper;
            _reposiotry = reposiotry;
        }
        public void AddStock(Stock stock)
        {
            var dto = _mapper.Map<StockDto>(stock);
            _reposiotry.Add(dto);
        }

        public void RemoveStock(Stock stock)
        {
            var dto = _mapper.Map<StockDto>(stock);
            _reposiotry.Remove(dto);
        }
    }
}