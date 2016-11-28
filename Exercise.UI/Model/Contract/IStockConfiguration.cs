namespace Exercise.UI.Model.Contract
{
    public interface IStockConfiguration
    {
        decimal GetTransactionCost(StockType stockType);
        decimal GetTolerance(StockType stockType);
    }
}