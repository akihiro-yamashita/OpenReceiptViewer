using System.Windows.Controls;
using System.Windows.Input;

namespace OpenReceiptViewer
{
    /// <summary>
    /// Viewer.xaml の相互作用ロジック
    /// </summary>
    public partial class Viewer : UserControl
    {
        private ViewerViewModel _vm;

        public Viewer()
        {
            InitializeComponent();

            _vm = new ViewerViewModel();
            this.DataContext = _vm;
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var obj = sender as DataGrid;
            if (obj != null && obj.SelectedItem != null)
            {
                obj.ScrollIntoView(obj.SelectedItem);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var modifyKey = Keyboard.Modifiers;

            if ((modifyKey & ModifierKeys.Control) != ModifierKeys.None)
            {
                if (e.Key == Key.F)
                {
                    var vm = this.DataContext as ViewerViewModel;
                    if (vm != null) { vm.NumberSearchCommand.Execute(null); }
                }
            }
        }
    }
}
