using System.ComponentModel;
using Exercise.UI.Extenxions;
using Exercise.UI.Model.Contract;
using Exercise.UI.ViewModel.EventHandlers;

namespace Exercise.UI.ViewModel
{
    public class StockSummary : INotifyPropertyChanged, INotifyPropertyDeltaChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyDeltaChangedEventHandler PropertyDeltaChanged;

        private StockType? _filter;
        private int _totalNumber;
        private decimal _totalStockWeight;
        private decimal _totalMarektValue;

        public StockSummary(StockType? filter = null)
        {
            Filter = filter;
        }

        public bool IsSummaryForStock(Stock stock)
        {
            return _filter == null || stock.StockType == Filter;
        }
        public string Name => Filter == null ? "All" : Filter.ToString();

        public StockType? Filter
        {
            get { return _filter; }
            private set { this.Notify(PropertyChanged, ref _filter, value); }
        }
        public int TotalNumber
        {
            get { return _totalNumber; }
            set { this.Notify(PropertyChanged, ref _totalNumber, value); }
        }
        public decimal TotalStockWeight
        {
            get { return _totalStockWeight; }
            set { this.Notify(PropertyChanged, PropertyDeltaChanged, ref _totalStockWeight, value); }
        }
        public decimal TotalMarektValue
        {
            get { return _totalMarektValue; }
            set { this.Notify(PropertyChanged, PropertyDeltaChanged, ref _totalMarektValue, value); }
        }
    }
}
