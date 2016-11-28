using System.Collections.Generic;
using Exercise.UI.Model.Contract;

namespace Exercise.UI.Model
{
    public class StockConfiguration : IStockConfiguration
    {
        public static readonly Dictionary<StockType, decimal> TransactionCost = new Dictionary<StockType, decimal>
        {
            [StockType.Equity] = 0.005M,
            [StockType.Bond] = 0.02M
        };

        public static readonly Dictionary<StockType, decimal> Tolerance = new Dictionary<StockType, decimal>
        {
            [StockType.Equity] = 200000,
            [StockType.Bond] = 100000
        };

        public decimal GetTransactionCost(StockType stockType)
        {
            return TransactionCost[stockType];
        }

        public decimal GetTolerance(StockType stockType)
        {
            return Tolerance[stockType];
        }
    }
}