/*
Copyright Since 2016 Akihiro Yamashita

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

using System.Collections.Generic;
using System.Windows;
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

            // メニュー生成
            Dictionary<string, RelayCommand> myMenuCommands;
            try
            {
                myMenuCommands = _vm.CreateMyMenuCommands();
            }
            catch
            {
                myMenuCommands = null;
                MessageBox.Show("マイメニューの読み込みに失敗しました。");
            }
            if (myMenuCommands != null && 0 < myMenuCommands.Count)
            {
                var menu = new ContextMenu();
                foreach (var kv in myMenuCommands)
                {
                    menu.Items.Add(new MenuItem()
                    {
                        Header = kv.Key,
                        Command = kv.Value,
                    });
                }
                _dataGrid.RowStyle = new Style(typeof(DataGridRow));
                _dataGrid.RowStyle.Setters.Add(new Setter(DataGrid.ContextMenuProperty, menu));
            }
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

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // DataGridにマウスホイールを取られるので、その親の親のScrollViewerに対してスクロールが効かない。
            // コードで無理矢理スクロール処理を行う。

            var dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                var stackPanel = dataGrid.Parent as StackPanel;
                if (stackPanel != null)
                {
                    var scrollViewer = stackPanel.Parent as ScrollViewer;
                    if (scrollViewer != null)
                    {
                        scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - e.Delta);
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
