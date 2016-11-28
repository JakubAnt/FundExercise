using System;
using System.ComponentModel;
using System.Windows.Input;
using Exercise.UI.Extenxions;
using Exercise.UI.Infrastructure;
using Exercise.UI.Model.Contract;

namespace Exercise.UI.ViewModel
{
    public class FundViewModel: INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Stock _stockToAdd;
        private Fund _fund;
        private ICommand _addStock;

        public FundViewModel(IStockService service, IFactory<Stock> stockFactory)
        {
            Fund = new Fund(service);
            StockToAdd = stockFactory.Create();
            AddStock = new AddStockCommand(StockToAdd, stockFactory);
        }

        public Stock StockToAdd
        {
            get { return _stockToAdd; }
            set { this.Notify(PropertyChanged, ref _stockToAdd, value); }
        }
        public Fund Fund
        {
            get { return _fund; }
            set { this.Notify(PropertyChanged, ref _fund, value); }
        }

        public ICommand AddStock
        {
            get { return _addStock; }
            set { this.Notify(PropertyChanged, ref _addStock, value); }
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
                Fund.Dispose();
            }

            _disposed = true;
        }
    }
}