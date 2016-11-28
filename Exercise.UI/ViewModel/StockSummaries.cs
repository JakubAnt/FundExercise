using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Exercise.UI.Extenxions;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel.EventHandlers;

namespace Exercise.UI.ViewModel
{
    public class StockSummaries : ObservableCollection<StockSummary>, IDisposable
    {
        private Stocks Stocks { get; }

        private StockSummary AllStockSummary { get; }

        public StockSummaries(Stocks stocks)
        {
            EnumExtension.GetValues<StockType>().Select(st => new StockSummary(st)).ToList().ForEach(Add);
            Stocks = stocks;
            AllStockSummary = stocks.StockSummary;
            Add(AllStockSummary);

            AllStockSummary.PropertyChanged += AllStockSummaryOnPropertyChanged;
            Stocks.CollectionChanged += StocksOnCollectionChanged;
        }

        private void StocksOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.SafeCast<Stock>().ToArray();
            var oldItems = args.OldItems.SafeCast<Stock>().ToArray();
            foreach (var added in newItems)
            {
                added.PropertyDeltaChanged += StockPropertyChanged;
            }
            foreach (var removed in oldItems)
            {
                removed.PropertyDeltaChanged-= StockPropertyChanged;
            }
            StocksChanged(newItems, oldItems);
        }

        private void StocksChanged(Stock[] added, Stock[]removed)
        {
            foreach (var summary in this)
            {
                var positive = added.Where(summary.IsSummaryForStock).ToArray();
                var negative = removed.Where(summary.IsSummaryForStock).ToArray();
                summary.TotalNumber += positive.Count();
                summary.TotalNumber -= negative.Count();
                summary.TotalMarektValue += positive.Sum(s => s.MarektValue);
                summary.TotalMarektValue -= negative.Sum(s => s.MarektValue);
            }
        }

        private void AllStockSummaryOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var allSummary = sender as StockSummary;
            foreach (var summary in this)
            {
                switch (args.PropertyName)
                {
                    case nameof(allSummary.TotalMarektValue):
                        //I was consider also absolute value to nicer handler for negative values, but form other hand, it could be business reason for keeping negative in weight.
                        summary.TotalStockWeight = allSummary.TotalStockWeight == 0 ? 0 : summary.TotalMarektValue/allSummary.TotalMarektValue;
                        break;
                }
            }
        }

        private void StockPropertyChanged(object sender, PropertyDeltaChangedEventArgs args)
        {
            var stock = sender as Stock;
            foreach (var summary in this.Where(t => t.IsSummaryForStock(stock)))
            {
                switch (args.PropertyName)
                {
                    case nameof(stock.MarektValue):
                        summary.TotalMarektValue += args.Delta; 
                        break;
                    case nameof(stock.StockWeight):
                        summary.TotalStockWeight += args.Delta; 
                        break;
                }
            }
        }

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
                AllStockSummary.PropertyChanged -= AllStockSummaryOnPropertyChanged;
                Stocks.CollectionChanged -= StocksOnCollectionChanged;
                foreach (var stock in Stocks)
                {
                    stock.PropertyDeltaChanged -= StockPropertyChanged;
                }
            }

            _disposed = true;
        }
    }
}