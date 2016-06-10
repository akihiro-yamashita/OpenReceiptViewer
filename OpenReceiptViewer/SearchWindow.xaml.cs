using System.Windows;

namespace OpenReceiptViewer
{
    /// <summary>
    /// NumberSearch.xaml の相互作用ロジック
    /// </summary>
    public partial class SearchWindow : Window
    {
        SearchWindowViewModel _vm;

        public SearchWindow()
        {
            InitializeComponent();

            _vm = new SearchWindowViewModel();
            this.DataContext = _vm;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
