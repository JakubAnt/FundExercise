using AutoMapper;
using Exercise.UI.Infrastructure;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel;

namespace Exercise.UI.Model
{
    public class MapperFactory : IFactory<IMapper>
    {
        public IMapper Create()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Stock, StockDto>());
            return config.CreateMapper();
        }
    }
}