using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Exercise.UI.Infrastructure;
using Exercise.UI.ViewModel;

namespace Exercise.UI
{
    /// <summary>
    /// Interaction logic for FundWindow.xaml
    /// </summary>
    public partial class FundWindow : Window, IDisposable
    {
        private FundViewModel _viewModel;
        public FundWindow(IFactory<FundViewModel> factory)
        {
            InitializeComponent();
            _viewModel = factory.Create();
            DataContext = _viewModel;
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
                _viewModel.Dispose();
            }

            _disposed = true;
        }
    }
}
