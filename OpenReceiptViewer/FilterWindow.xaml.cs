using System.Windows;

namespace OpenReceiptViewer
{
    /// <summary>
    /// FilterWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FilterWindow : Window
    {
        FilterWindowViewModel _vm;

        public FilterWindow()
        {
            InitializeComponent();

            _vm = new FilterWindowViewModel();
            this.DataContext = _vm;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
