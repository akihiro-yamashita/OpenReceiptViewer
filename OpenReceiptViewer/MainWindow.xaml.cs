/*
Copyright 2016 Akihiro Yamashita

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;

namespace OpenReceiptViewer
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowViewModel _vm;

		public MainWindow()
		{
			InitializeComponent();

            this.DataContext = this;

            this._vm = new MainWindowViewModel();
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

        private void TabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 「＋」のタブを選択状態にさせない。
            e.Handled = true;
        }

        //private void _tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var addNewTab = _tabControl.Items[_tabControl.Items.Count - 1] as TabItem;
        //    if (addNewTab != null)
        //    {
        //        addNewTab.IsSelected = false;
        //    }
        //}

        //private void _main_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //	ModifierKeys modifyKey = Keyboard.Modifiers;

        //	if ((modifyKey & ModifierKeys.Control) != ModifierKeys.None)
        //	{
        //		if (e.Key == Key.F) {
        //			ViewModel vm = this.DataContext as ViewModel;
        //			if (vm != null) { vm.NumberSearchCommand.Execute(null); }
        //		}
        //	}
        //}
    }
}
