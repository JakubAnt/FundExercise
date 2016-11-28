using System;
using Exercise.UI.Model.Contract;

namespace Exercise.UI.ViewModel
{
    public class Fund : IDisposable
    {
        public Fund(IStockService service)
        {
            Stocks = new Stocks(service);
            Summaries = new StockSummaries(Stocks);
        }

        public Stocks Stocks { get; private set; }
        public StockSummaries Summaries { get; private set; }

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Stocks.Dispose();
                Summaries.Dispose();
            }

            _disposed = true;
        }
    }
}