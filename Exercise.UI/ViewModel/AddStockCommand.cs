using System;
using System.ComponentModel;
using System.Windows.Input;
using Exercise.UI.Infrastructure;

namespace Exercise.UI.ViewModel
{
    public class AddStockCommand : ICommand, IDisposable
    {
        private readonly IFactory<Stock> _stockFactory;
        public event EventHandler CanExecuteChanged;
        private INotifyPropertyChanged _subscribedForCanExecute;

        public AddStockCommand(Stock stockToAdd, IFactory<Stock> stockFactory)
        {
            _stockFactory = stockFactory;
            SubscribedForCanExecute = stockToAdd;
        }

        protected INotifyPropertyChanged SubscribedForCanExecute
        {
            get { return _subscribedForCanExecute; }
            set
            {
                if (_subscribedForCanExecute != null)
                {
                    _subscribedForCanExecute.PropertyChanged -= UpdateCanExecuteState;
                }
                _subscribedForCanExecute = value;
                if (_subscribedForCanExecute != null)
                {
                    _subscribedForCanExecute.PropertyChanged += UpdateCanExecuteState;
                }
            }
        }

        public void UpdateCanExecuteState(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Stock.Price) || e.PropertyName == nameof(Stock.Quantity))
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            var viewModel = parameter as FundViewModel;
            return viewModel != null && viewModel.StockToAdd.Price != 0 && viewModel.StockToAdd.Quantity != 0;
        }

        public void Execute(object parameter)
        {
            var viewModel = parameter as FundViewModel;
            if (viewModel == null)
            {
                return;
            }
            viewModel.Fund.Stocks.Add(viewModel.StockToAdd);
            var nextElement = _stockFactory.Create();
            SubscribedForCanExecute = nextElement;
            viewModel.StockToAdd = nextElement;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
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
                SubscribedForCanExecute = null;
            }

            _disposed = true;
        }

    }

}