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

using System;
using System.Windows;
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

            this._vm = new MainWindowViewModel();
            this.DataContext = _vm;

            var args = Environment.GetCommandLineArgs();
            if (1 < args.Length)
            {
                var filePath = args[1];
                _vm.Open(_tabControl, filePath);
            }
        }

        private void TabItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source != null && e.Source.GetType() == typeof(Button))
            {
            }
            else
            {
                // 「＋」のタブを選択状態にさせないため、ボタン以外の部分を押されても無視。
                e.Handled = true;
            }
        }

        private void TabItem_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var filePathArray = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (0 < filePathArray.Length)
            {
                var filePath = filePathArray[0];

                var tabItem = sender as TabItem;
                if (tabItem != null)
                {
                    var tabControl = tabItem.Parent as TabControl;
                    if (tabControl != null)
                    {
                        _vm.Open(tabControl, filePath);
                    }
                }
            }
        }
    }
}
