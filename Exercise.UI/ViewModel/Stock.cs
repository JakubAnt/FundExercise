using System.ComponentModel;
using Exercise.UI.Extenxions;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel.EventHandlers;

namespace Exercise.UI.ViewModel
{
    public class Stock : INotifyPropertyChanged, INotifyPropertyDeltaChanged
    {
        private readonly IStockConfiguration _configuration;
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyDeltaChangedEventHandler PropertyDeltaChanged;

        private StockType _stockType;
        private string _name;
        private decimal _price;
        private int _quantity;
        private decimal _marektValue;
        private decimal _stockWeight;
        private decimal _transactionCost;
        private bool _isInTolerance = true;

        public Stock(IStockConfiguration configuration)
        {
            _configuration = configuration;
            PropertyChanged += Recalculate;
        }
    
        public StockType StockType
        {
            get { return _stockType; }
            set { this.Notify(PropertyChanged, ref _stockType, value); }
        }
        public string Name
        {
            get { return _name; }
            set { this.Notify(PropertyChanged, ref _name, value); }
        }
        public decimal Price
        {
            get { return _price; }
            set { this.Notify(PropertyChanged, PropertyDeltaChanged, ref _price, value); }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { this.Notify(PropertyChanged, ref _quantity, value); }
        }
        public decimal MarektValue
        {
            get { return _marektValue; }
            set { this.Notify(PropertyChanged, PropertyDeltaChanged, ref _marektValue, value); }
        }
        public decimal StockWeight
        {
            get { return _stockWeight; }
            set { this.Notify(PropertyChanged, PropertyDeltaChanged, ref _stockWeight, value); }
        }
        public decimal TransactionCost
        {
            get { return _transactionCost; }
            set { this.Notify(PropertyChanged, PropertyDeltaChanged, ref _transactionCost, value); }
        }

        public bool IsInTolerance
        {
            get { return _isInTolerance; }
            set { this.Notify(PropertyChanged, ref _isInTolerance, value); }
        }

        private void Recalculate(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(Price):
                case nameof(Quantity):
                    MarektValue = Price * Quantity;
                    break;
                case nameof(MarektValue):
                    TransactionCost = GetTransactionCost();
                    IsInTolerance = GetToleranceFlag();
                    break;
                case nameof(StockType):
                    TransactionCost = GetTransactionCost();
                    break;
                case nameof(TransactionCost):
                    IsInTolerance = GetToleranceFlag();
                    break;
            }
        }

        private decimal GetTransactionCost()
        {
            return MarektValue * _configuration.GetTransactionCost(StockType);
        }

        private bool GetToleranceFlag()
        {
            return MarektValue >= 0 && TransactionCost <= _configuration.GetTolerance(StockType);
        }
    }
}
