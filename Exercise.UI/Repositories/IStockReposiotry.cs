using Exercise.UI.Model.Contract;

namespace Exercise.UI.Repositories
{
    public interface IStockReposiotry
    {
        void Add(StockDto stock);
        void Remove(StockDto stock);
    }
}