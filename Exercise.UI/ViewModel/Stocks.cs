using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Exercise.UI.Extenxions;
using Exercise.UI.Model.Contract;

namespace Exercise.UI.ViewModel
{
    public class Stocks : ObservableCollection<Stock>, IDisposable
    {
        private readonly IStockService _service;
        public Dictionary<StockType, int> NameSufixs { get; }

        public Stocks(IStockService service)
        {
            _service = service;
            NameSufixs = EnumExtension.GetValues<StockType>().ToDictionary(e => e, _ => 1);
            StockSummary = new StockSummary();
            StockSummary.PropertyChanged += RecalculateWeightOnStocks;
            base.CollectionChanged += OnCollectionChanged;
            base.CollectionChanged += Save;
        }

        private void Save(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems.SafeCast<Stock>().ToList();
            var oldItems = args.OldItems.SafeCast<Stock>().ToList();

            newItems.ForEach(s => _service.AddStock(s));
            oldItems.ForEach(s => _service.RemoveStock(s));
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach (var added in args.NewItems.SafeCast<Stock>())
            {
                var number = NameSufixs[added.StockType]++;
                added.Name = added.Name ?? $"{added.StockType}{number}";
            }
        }

        public StockSummary StockSummary { get; private set; }

        private void RecalculateWeightOnStocks(object sender, PropertyChangedEventArgs args)
        {
            var summary = sender as StockSummary;
            switch (args.PropertyName)
            {
                case nameof(summary.TotalMarektValue):
                    foreach (var stock in this)
                    {
                        stock.StockWeight = summary.TotalMarektValue == 0 ? 0 : (stock.MarektValue / summary.TotalMarektValue);
                    }
                    break;
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
                base.CollectionChanged -= OnCollectionChanged;
                StockSummary.PropertyChanged -= RecalculateWeightOnStocks;
            }
          
            _disposed = true;
        }
    }
}