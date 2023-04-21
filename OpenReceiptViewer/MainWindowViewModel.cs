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

using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace OpenReceiptViewer
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
        }

        /// <summary></summary>
        public RelayCommand<TabControl> OpenCommand
        {
            get
            {
                return _openCommand = _openCommand ??
                new RelayCommand<TabControl>(tabControl =>
                {
                    var dialog = new Microsoft.Win32.OpenFileDialog();
                    dialog.Filter = "*.UKE,*.HEN,*.SAH|*.*";
                    dialog.FilterIndex = 0;
                    var dialogResult = dialog.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        Open(tabControl, dialog.FileName);
                    }
                });
            }
        }
        private RelayCommand<TabControl> _openCommand;

        public void Open(TabControl tabControl, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            var viewer = new Viewer();
            var vm = viewer.DataContext as ViewerViewModel;
            vm.ReceiptFilePath = filePath;
            vm.OpenCommand.Execute(null);
            var tabItem = new TabItem { Content = viewer, };
            var header = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        Content = vm.IRHI.審査支払機関.ToString() + "  請求年月" + DateUtil.ReceiptDateToShowDate(vm.IRHI.請求年月, true),
                    },
                    new Button
                    {
                        Content = "×",
                        BorderBrush = System.Windows.Media.Brushes.Transparent,
                        Height = 20,
                        Width = 20,
                        ToolTip = "閉じる",
                        Command = CloseCommand,
                        CommandParameter = tabItem,
                    },
                },
                ToolTip = vm.ReceiptFilePath,
            };
            tabItem.Header = header;
            tabControl.Items.Insert(tabControl.Items.Count - 1, tabItem);
            //tabControl.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        /// <summary></summary>
        public RelayCommand<TabItem> CloseCommand
        {
            get
            {
                return _closeCommand = _closeCommand ??
                new RelayCommand<TabItem>(tabItem =>
                {
                    var tabControl = tabItem.Parent as TabControl;
                    if (tabControl != null && tabItem != null)
                    {
                        tabControl.Items.Remove(tabItem);

                        // タブの最後の「＋」が選択状態になってしまった場合
                        if (tabControl.SelectedIndex == tabControl.Items.Count - 1)
                        {
                            // 「＋」より手前のタブを選択
                            if (1 < tabControl.Items.Count)
                            {
                                var previousTab = tabControl.Items[tabControl.Items.Count - 2] as TabItem;
                                previousTab.IsSelected = true;
                            }
                        }
                    }
                });
            }
        }
        private RelayCommand<TabItem> _closeCommand;

    }
}
