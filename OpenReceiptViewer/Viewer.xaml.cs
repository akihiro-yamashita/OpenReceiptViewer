using System.Windows.Controls;
using System.Windows.Input;

namespace OpenReceiptViewer
{
    /// <summary>
    /// Viewer.xaml の相互作用ロジック
    /// </summary>
    public partial class Viewer : UserControl
    {
        ViewerViewModel _vm;

        public Viewer()
        {
            InitializeComponent();

            _vm = new ViewerViewModel();
            this.DataContext = _vm;
        }

        private void DataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            var obj = sender as System.Windows.Controls.DataGrid;
            if (obj != null && obj.SelectedItem != null)
            {
                obj.ScrollIntoView(obj.SelectedItem);
            }
        }

        private void _main_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
