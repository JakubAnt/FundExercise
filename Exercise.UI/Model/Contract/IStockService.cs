using Exercise.UI.ViewModel;

namespace Exercise.UI.Model.Contract
{
    public interface IStockService
    {
        void AddStock(Stock stock);

        void RemoveStock(Stock stock);
    }
}