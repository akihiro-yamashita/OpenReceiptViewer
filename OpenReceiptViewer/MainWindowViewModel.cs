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

using CsvHelper;
using System;
using System.Collections.Generic;
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
                    if (string.IsNullOrEmpty(MasterDiretoryPath))
                    {
                        MasterDiretoryPath = @"Master\201804";
                        InitDict();
                    }

                    var dialog = new Microsoft.Win32.OpenFileDialog();
                    dialog.Filter = "*.UKE|*.*";
                    dialog.FilterIndex = 0;
                    var dialogResult = dialog.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        var viewer = new Viewer();
                        var vm = viewer.DataContext as ViewerViewModel;
                        vm.MasterDiretoryPath = this.MasterDiretoryPath;
                        vm.ReceiptFilePath = dialog.FileName;
                        vm.OpenCommand.Execute(null);
                        var header = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Children =
                            {
                                new Label{ Content = vm.IR.審査支払機関.ToString() + "  請求年月" + DateUtil.ReceiptDateToShowDate(vm.IR.請求年月, true), },
                                new Button
                                {
                                    Content = "×",
                                    BorderBrush = System.Windows.Media.Brushes.Transparent,
                                    Height = 20,
                                    Width = 20,
                                    ToolTip = "閉じる",
                                    Command = CloseCommand,
                                    CommandParameter = tabControl,
                                },
                            },
                        };
                        var tabItem = new TabItem { Header = header, Content = viewer, };
                        tabControl.Items.Insert(tabControl.Items.Count - 1, tabItem);
                        //tabControl.Items.Add(tabItem);
                        tabItem.IsSelected = true;
                    }
                });
            }
        }
        private RelayCommand<TabControl> _openCommand;

        /// <summary></summary>
        public RelayCommand<TabControl> CloseCommand
        {
            get
            {
                return _closeCommand = _closeCommand ??
                new RelayCommand<TabControl>(tabControl =>
                {
                    if (0 <= tabControl.SelectedIndex)
                    {
                        tabControl.Items.RemoveAt(tabControl.SelectedIndex);
                    }
                });
            }
        }
        private RelayCommand<TabControl> _closeCommand;

        void InitDict()
        {
            傷病名Converter.Instance.傷病名Dict = this.Read傷病名();
            傷病名Converter.Instance.修飾語Dict = this.Read修飾語();
            コメントConverter.Instance.コメントDict = this.Readコメント();

            var 診療行為List = this.Read診療行為();
            DictConverter.診療行為Instance.Dict = 診療行為List.ToDictionary(x => x.Id, x => x.名称);
            DictConverter.診療行為単位Instance.Dict = 診療行為List.ToDictionary(x => x.Id, x => x.単位);
            var 医薬品List = this.Read医薬品();
            DictConverter.医薬品Instance.Dict = 医薬品List.ToDictionary(x => x.Id, x => x.名称);
            DictConverter.医薬品単位Instance.Dict = 医薬品List.ToDictionary(x => x.Id, x => x.単位);
            var 特定器材List = this.Read特定器材();
            DictConverter.特定器材Instance.Dict = 特定器材List.ToDictionary(x => x.Id, x => x.名称);
            DictConverter.特定器材単位Instance.Dict = 特定器材List.ToDictionary(x => x.Id, x => x.単位);
        }

        public string MasterDiretoryPath { get; set; }

        private Dictionary<int, string> Read傷病名()
        {
            var filePath = System.IO.Path.Combine(MasterDiretoryPath, "b.csv");

            var dict = new Dictionary<int, string>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var id = csv.GetField<int>(2);
                    var name = csv.GetField<string>(5);
                    dict.Add(id, name);
                }
            };
            CSVUtil.Read(filePath, readAction);
            return dict;
        }

        private Dictionary<int, string> Read修飾語()
        {
            var filePath = System.IO.Path.Combine(MasterDiretoryPath, "z.csv");

            var dict = new Dictionary<int, string>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var id = csv.GetField<int>(2);
                    var name = csv.GetField<string>(6);
                    dict.Add(id, name);
                }
            };
            CSVUtil.Read(filePath, readAction);
            return dict;
        }

        private List<名称単位マスター> Read名称単位マスター(string fileName)
        {
            var filePath = System.IO.Path.Combine(MasterDiretoryPath, fileName);

            var list = new List<名称単位マスター>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var id = csv.GetField<int>(2);
                    var 名称 = csv.GetField<string>(4);
                    var 単位 = csv.GetField<string>(9);
                    list.Add(new 名称単位マスター() { Id = id, 名称 = 名称, 単位 = 単位 });
                }
            };
            CSVUtil.Read(filePath, readAction);
            return list;
        }

        private List<名称単位マスター> Read診療行為()
        {
            return Read名称単位マスター("s.csv");
        }

        private List<名称単位マスター> Read医薬品()
        {
            return Read名称単位マスター("y.csv");
        }

        private List<名称単位マスター> Read特定器材()
        {
            return Read名称単位マスター("t.csv");
        }

        private Dictionary<int, string> Readコメント()
        {
            var filePath = System.IO.Path.Combine(MasterDiretoryPath, "c.csv");

            var dict = new Dictionary<int, string>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var keta1 = csv.GetField<int>(2);
                    var keta23 = csv.GetField<int>(3);
                    var keta89 = csv.GetField<int>(4);
                    var str = csv.GetField<string>(6);
                    var id = (keta1 * 100000000) + (keta23 * 1000000) + keta89;
                    dict.Add(id, str);
                }
            };
            CSVUtil.Read(filePath, readAction);
            return dict;
        }
    }
}
