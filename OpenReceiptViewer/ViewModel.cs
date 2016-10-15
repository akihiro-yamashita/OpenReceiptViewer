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
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;

namespace OpenReceiptViewer
{
    public class ViewModel : NotifyPropertyChanged
    {
        /// <summary></summary>
        public IR IR { get; set; }

        /// <summary></summary>
        public ObservableCollection<Patient> PatientList { get; set; }

        /// <summary>条件使用時のPatientList退避用</summary>
        public List<Patient> PatientListOriginal { get; set; }

        /// <summary></summary>
        public ObservableCollection<SY> SYList { get; set; }

        /// <summary></summary>
        public ObservableCollection<SIIYTOCO> SIIYTOCOList { get; set; }

        public Patient CurrentPatient
        {
            get
            {
                return this._currentPatient;
            }
            set
            {
                this._currentPatient = value;
                OnPropertyChanged("CurrentPatient");

                if (value == null) { return; }

                Tuple<List<SY>, List<SIIYTOCO>> tuple;
                try
                {
                    tuple = this.ReadOneReceipt(ReceiptFilePath, value.RE.レセプト番号);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                this.SYList.Clear();
                tuple.Item1.ForEach(x => this.SYList.Add(x));
                this.SIIYTOCOList.Clear();
                tuple.Item2.ForEach(x => this.SIIYTOCOList.Add(x));
            }
        }
        private Patient _currentPatient;

        private string ReceiptFilePath = string.Empty;
        private string MasterDiretoryPath = "Master";

        public ViewModel()
        {
            this.IR = new IR();
            this.PatientList = new ObservableCollection<Patient>();
            this.PatientListOriginal = null;
            this.SYList = new ObservableCollection<SY>();
            this.SIIYTOCOList = new ObservableCollection<SIIYTOCO>();

            new Thread(new ThreadStart(() =>
            {
                傷病名Converter.Instance.傷病名Dict = this.Read傷病名();
                傷病名Converter.Instance.修飾語Dict = this.Read修飾語();
                コメントConverter.Instance.コメントDict = this.Readコメント();
            })).Start();

            new Thread(new ThreadStart(() =>
            {
                var 診療行為List = this.Read診療行為();
                DictConverter.診療行為Instance.Dict = 診療行為List.ToDictionary(x => x.Id, x => x.名称);
                DictConverter.診療行為単位Instance.Dict = 診療行為List.ToDictionary(x => x.Id, x => x.単位);
                var 医薬品List = this.Read医薬品();
                DictConverter.医薬品Instance.Dict = 医薬品List.ToDictionary(x => x.Id, x => x.名称);
                DictConverter.医薬品単位Instance.Dict = 医薬品List.ToDictionary(x => x.Id, x => x.単位);
                var 特定器材List = this.Read特定器材();
                DictConverter.特定器材Instance.Dict = 特定器材List.ToDictionary(x => x.Id, x => x.名称);
                DictConverter.特定器材単位Instance.Dict = 特定器材List.ToDictionary(x => x.Id, x => x.単位);
            })).Start();
        }

        /// <summary></summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand = _openCommand ??
                new RelayCommand(() =>
                {
                    var dialog = new Microsoft.Win32.OpenFileDialog();
                    dialog.Filter = "*.UKE|*.*";
                    dialog.FilterIndex = 0;
                    var dialogResult = dialog.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        this.ReceiptFilePath = dialog.FileName;
                        Tuple<IR, List<Patient>> tuple;
                        try
                        {
                            tuple = ReadReceiptSummary(ReceiptFilePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }

                        //// バインドが切れてしまう。
                        //this.IR = tuple.Item1;
                        this.IR.審査支払機関 = tuple.Item1.審査支払機関;
                        this.IR.医療機関名称 = tuple.Item1.医療機関名称;
                        this.IR.請求年月 = tuple.Item1.請求年月;

                        this.PatientList.Clear();
                        tuple.Item2.ForEach(x => this.PatientList.Add(x));
                        this.PatientListOriginal = null;
                        this.SYList.Clear();
                        this.SIIYTOCOList.Clear();
                    }
                });
            }
        }
        private RelayCommand _openCommand;

        /// <summary></summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand = _closeCommand ??
                new RelayCommand(() =>
                {
                    this.IR.審査支払機関 = 審査支払機関.社保;
                    this.IR.医療機関名称 = "";
                    this.IR.請求年月 = 0;

                    this.PatientList.Clear();
                    this.PatientListOriginal = null;
                    this.SYList.Clear();
                    this.SIIYTOCOList.Clear();
                });
            }
        }
        private RelayCommand _closeCommand;

        /// <summary>患者番号検索</summary>
        public RelayCommand NumberSearchCommand
        {
            get
            {
                return _numberSearchCommand = _numberSearchCommand ??
                new RelayCommand(() =>
                {
                    if (this.PatientList.Count == 0) { return; }

                    var window = new SearchWindow();
                    window.Title = "患者番号検索";
                    window.Label.Content = "患者番号";
                    var dialogResult = window.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        var input = ((SearchWindowViewModel)window.DataContext).Input;
                        int 患者番号;
                        if (Int32.TryParse(input, out 患者番号))
                        {
                            var result = PatientList.FirstOrDefault(x => x.RE.患者番号 == 患者番号, CurrentPatient);
                            if (result == null)
                            {
                                MessageBox.Show("指定された患者番号は見つかりませんでした。");
                            }
                            else
                            {
                                this.CurrentPatient = result;
                            }
                        }
                    }
                });
            }
        }
        private RelayCommand _numberSearchCommand;

        /// <summary>氏名検索</summary>
        public RelayCommand NameSearchCommand
        {
            get
            {
                return _nameSearchCommand = _nameSearchCommand ??
                new RelayCommand(() =>
                {
                    if (this.PatientList.Count == 0) { return; }

                    var window = new SearchWindow();
                    window.Title = "氏名検索";
                    window.Label.Content = "氏名";
                    var dialogResult = window.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        var input = ((SearchWindowViewModel)window.DataContext).Input;
                        if (!string.IsNullOrEmpty(input))
                        {
                            var result = PatientList.FirstOrDefault(x => x.RE.氏名.Contains(input), CurrentPatient);
                            if (result == null)
                            {
                                MessageBox.Show("指定された患者は見つかりませんでした。");
                            }
                            else
                            {
                                this.CurrentPatient = result;
                            }
                        }
                    }
                });
            }
        }
        private RelayCommand _nameSearchCommand;

        /// <summary>条件解除</summary>
        public RelayCommand ClearFilterCommand
        {
            get
            {
                return _clearFilterCommand = _clearFilterCommand ??
                new RelayCommand(() =>
                {
                    if (this.PatientListOriginal == null)
                    {
                        return;
                    }

                    this.PatientList.Clear();
                    foreach (var re in this.PatientListOriginal)
                    {
                        this.PatientList.Add(re);
                    }
                    this.PatientListOriginal = null;
                });
            }
        }
        private RelayCommand _clearFilterCommand;

        private void FilterAction(string レコード識別情報)
        {
            if (this.PatientList.Count == 0) { return; }

            var レコード識別名称 = (string)null;
            var idField = 0;
            var fileName = (string)null;
            if (レコード識別情報 == レコード識別情報定数.診療行為)
            {
                レコード識別名称 = "診療行為";
                idField = (int)SI_IY_IDX.診療行為または医薬品コード;
                fileName = "s.csv";
            }
            else if (レコード識別情報 == レコード識別情報定数.医薬品)
            {
                レコード識別名称 = "医薬品";
                idField = (int)SI_IY_IDX.診療行為または医薬品コード;
                fileName = "y.csv";
            }
            else if (レコード識別情報 == レコード識別情報定数.特定器材)
            {
                レコード識別名称 = "特定器材";
                idField = (int)TO_IDX.特定器材コード;
                fileName = "t.csv";
            }
            else
            {
                return;
            }

            var window = new FilterWindow();
            window.Title = レコード識別名称 + "条件";
            window.Label.Content = レコード識別名称 + "（前方一致）";
            var input = (string)null;
            var dialogResult = window.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                input = ((FilterWindowViewModel)window.DataContext).Input;
                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            var masterFilePath = System.IO.Path.Combine(MasterDiretoryPath, fileName);
            var masterIds = new Dictionary<int, int>();
            Action<CsvReader> sReadAction = csv =>
            {
                while (csv.Read())
                {
                    var id = csv.GetField<int>(2);
                    var name = csv.GetField<string>(4);
                    if (name.StartsWith(input))
                    {
                        masterIds.Add(id, id);
                    }
                }
            };
            this.Read(masterFilePath, sReadAction);
            if (masterIds.Count == 0)
            {
                MessageBox.Show("「" + input + "」から始まる" + レコード識別名称 + "マスターがありません。");
                return;
            }

            // 一旦既存条件クリア
            this.ClearFilterCommand.Execute(null);

            var dict = new Dictionary<int, int>();
            Action<CsvReader> readAction = csv =>
            {
                var currentReceiptNo = -1;
                while (csv.Read())
                {
                    var lineDef = csv.GetField<string>(0);
                    if (lineDef == レコード識別情報定数.レセプト共通)
                    {
                        currentReceiptNo = csv.GetField<int>((int)RE_IDX.レセプト番号);
                    }
                    else if (lineDef == レコード識別情報)
                    {
                        if (dict.ContainsKey(currentReceiptNo))
                        {
                            continue;
                        }

                        var id = (int)csv.GetField<int>(idField);
                        if (masterIds.ContainsKey(id))
                        {
                            dict.Add(currentReceiptNo, currentReceiptNo);
                        }
                    }
                }
            };
            this.Read(ReceiptFilePath, readAction);

            // オリジナル保存
            this.PatientListOriginal = new List<Patient>(this.PatientList);

            // 条件一致のサマリー行のみ再追加
            this.PatientList.Clear();
            foreach (var patient in this.PatientListOriginal)
            {
                if (dict.ContainsKey(patient.RE.レセプト番号))
                {
                    this.PatientList.Add(patient);
                }
            }
        }

        /// <summary>診療行為条件</summary>
        public RelayCommand 診療行為FilterCommand
        {
            get
            {
                return _診療行為FilterCommand = _診療行為FilterCommand ??
                new RelayCommand(() =>
                {
                    this.FilterAction(レコード識別情報定数.診療行為);
                });
            }
        }
        private RelayCommand _診療行為FilterCommand;

        /// <summary>医薬品条件</summary>
        public RelayCommand 医薬品FilterCommand
        {
            get
            {
                return _医薬品FilterCommand = _医薬品FilterCommand ??
                new RelayCommand(() =>
                {
                    this.FilterAction(レコード識別情報定数.医薬品);
                });
            }
        }
        private RelayCommand _医薬品FilterCommand;

        /// <summary>特定器材条件</summary>
        public RelayCommand 特定器材FilterCommand
        {
            get
            {
                return _特定器材FilterCommand = _特定器材FilterCommand ??
                new RelayCommand(() =>
                {
                    this.FilterAction(レコード識別情報定数.特定器材);
                });
            }
        }
        private RelayCommand _特定器材FilterCommand;

        private Tuple<IR, List<Patient>> ReadReceiptSummary(string filePath)
        {
            var ir = new IR();
            var patientList = new List<Patient>();
            Action<CsvReader> readAction = csv =>
            {
                var patient = (Patient)null;

                Action add = () =>
                {
                    patientList.Add(patient);
                    patient = null;
                };

                while (csv.Read())
                {
                    var lineDef = csv.GetField<string>(0);
                    if (lineDef == レコード識別情報定数.医療機関情報)
                    {
                        ir.審査支払機関 = (審査支払機関)csv.GetField<int>((int)IR_IDX.審査支払機関);
                        ir.都道府県 = csv.GetField<int>((int)IR_IDX.都道府県);
                        ir.点数表 = csv.GetField<int>((int)IR_IDX.点数表);
                        ir.医療機関コード = csv.GetField<int>((int)IR_IDX.医療機関コード);
                        ir.予備 = csv.GetField<int?>((int)IR_IDX.予備);
                        ir.医療機関名称 = csv.GetField<string>((int)IR_IDX.医療機関名称);
                        ir.請求年月 = csv.GetField<int>((int)IR_IDX.請求年月);
                        ir.マルチボリューム識別子 = csv.GetField<int>((int)IR_IDX.マルチボリューム識別子);
                        ir.電話番号 = csv.GetField<string>((int)IR_IDX.電話番号);
                        ir.医療機関名称 = csv.GetField<string>((int)IR_IDX.医療機関名称);
                    }
                    else if (lineDef == レコード識別情報定数.レセプト共通)
                    {
                        if (patient != null)
                        {
                            add();  // 前の患者を追加する。
                        }

                        var re = new RE()
                        {
                            レセプト番号 = csv.GetField<int>((int)RE_IDX.レセプト番号),
                            レセプト種別 = csv.GetField<int>((int)RE_IDX.レセプト種別),
                            診療年月 = csv.GetField<int>((int)RE_IDX.診療年月),
                            氏名 = csv.GetField<string>((int)RE_IDX.氏名),
                            男女区分 = (男女区分)csv.GetField<int>((int)RE_IDX.男女区分),
                            生年月日 = csv.GetField<int>((int)RE_IDX.生年月日),
                            患者番号 = csv.GetField<int>((int)RE_IDX.カルテ番号等),
                        };
                        patient = new Patient();
                        patient.RE = re;
                    }
                    else if (lineDef == レコード識別情報定数.保険者)
                    {
                        var ho = new HO()
                        {
                            保険者番号 = csv.GetField<int>((int)HO_IDX.保険者番号),
                            被保険者証記号 = csv.GetField<string>((int)HO_IDX.被保険者証記号),
                            被保険者証番号 = csv.GetField<string>((int)HO_IDX.被保険者証番号),
                            診療実日数 = csv.GetField<int>((int)HO_IDX.診療実日数),
                            合計点数 = csv.GetField<int>((int)HO_IDX.合計点数),
                            予備 = csv.GetField<int?>((int)HO_IDX.予備),
                            回数 = csv.GetField<int?>((int)HO_IDX.回数),
                            //合計金額 = csv.GetField<int?>((int)HO_IDX.合計金額),
                            職務上の事由 = csv.GetField<int?>((int)HO_IDX.職務上の事由),
                            証明証番号 = csv.GetField<int?>((int)HO_IDX.証明証番号),
                            医療保険 = csv.GetField<int?>((int)HO_IDX.医療保険),
                            減免区分 = csv.GetField<int?>((int)HO_IDX.減免区分),
                            減額割合 = csv.GetField<int?>((int)HO_IDX.減額割合),
                            減額金額 = csv.GetField<int?>((int)HO_IDX.減額金額),
                        };
                        if (patient != null && patient.RE != null)
                        {
                            patient.HO = ho;
                        }
                        else
                        {
                            Debug.Assert(false, "保険者レコードの順番が不正です。");
                        }
                    }
                    else if (lineDef == レコード識別情報定数.公費)
                    {
                        var ko = new KO()
                        {
                            負担者番号 = csv.GetField<string>((int)KO_IDX.負担者番号),
                            受給者番号 = csv.GetField<int?>((int)KO_IDX.受給者番号),
                            任意給付区分 = csv.GetField<int?>((int)KO_IDX.任意給付区分),
                            診療実日数 = csv.GetField<int>((int)KO_IDX.診療実日数),
                            合計点数 = csv.GetField<int>((int)KO_IDX.合計点数),
                            公費 = csv.GetField<int?>((int)KO_IDX.公費),
                            外来一部負担金 = csv.GetField<int?>((int)KO_IDX.外来一部負担金),
                            入院一部負担金 = csv.GetField<int?>((int)KO_IDX.入院一部負担金),
                            予備 = csv.GetField<int?>((int)KO_IDX.予備),
                            回数 = csv.GetField<int?>((int)KO_IDX.回数),
                            合計金額 = csv.GetField<int?>((int)KO_IDX.合計金額),
                        };
                        if (patient != null && patient.RE != null)
                        {
                            patient.KO = ko;
                        }
                        else
                        {
                            Debug.Assert(false, "公費レコードの順番が不正です。");
                        }
                    }
                }

                add();  // 最後の患者を追加する。
            };
            this.Read(filePath, readAction);
            return new Tuple<IR, List<Patient>>(ir, patientList);
        }

        private Tuple<List<SY>, List<SIIYTOCO>> ReadOneReceipt(string filePath, int レセプト番号)
        {
            var syList = new List<SY>();
            var siiytocoList = new List<SIIYTOCO>();
            Action<CsvReader> readAction = csv =>
            {
                var reading = false;
                while (csv.Read())
                {
                    var lineDef = csv.GetField<string>(0);
                    if (lineDef == レコード識別情報定数.レセプト共通)
                    {
                        var currentReceiptNo = csv.GetField<int>((int)RE_IDX.レセプト番号);
                        if (currentReceiptNo == レセプト番号)
                        {
                            if (!reading)
                            {
                                // 傷病名レコード読み込み開始
                                reading = true;
                            }
                        }
                        else
                        {
                            if (reading)
                            {
                                // 傷病名レコード読み込み終了
                                reading = false;
                                break;
                            }
                        }
                    }
                    else if (reading && lineDef == レコード識別情報定数.傷病名)
                    {
                        var sy = new SY()
                        {
                            傷病名コード = csv.GetField<int>((int)SY_IDX.傷病名コード),
                            診療開始日 = csv.GetField<int>((int)SY_IDX.診療開始日),
                            転帰区分 = (転帰区分)csv.GetField<int>((int)SY_IDX.転帰区分),
                            修飾語コード = csv.GetField<string>((int)SY_IDX.修飾語コード),
                            傷病名称 = csv.GetField<string>((int)SY_IDX.傷病名称),
                            主傷病 = csv.GetField<string>((int)SY_IDX.主傷病),
                            補足コメント = csv.GetField<string>((int)SY_IDX.補足コメント),
                        };
                        syList.Add(sy);
                    }
                    else if (reading
                        && (lineDef == レコード識別情報定数.診療行為
                        || lineDef == レコード識別情報定数.医薬品))
                    {
                        SIIYTO siiyto;
                        if (lineDef == レコード識別情報定数.診療行為)
                        {
                            siiyto = new SI();
                        }
                        else if (lineDef == レコード識別情報定数.医薬品)
                        {
                            siiyto = new IY();
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        siiyto.診療識別 = csv.GetField<int?>((int)SI_IY_IDX.診療識別);
                        siiyto.負担区分 = csv.GetField<string>((int)SI_IY_IDX.負担区分);
                        siiyto.コード = (int)csv.GetField<int>((int)SI_IY_IDX.診療行為または医薬品コード);
                        siiyto.数量 = csv.GetField<float?>((int)SI_IY_IDX.数量);
                        siiyto.点数 = csv.GetField<int?>((int)SI_IY_IDX.点数);
                        siiyto.回数 = csv.GetField<int>((int)SI_IY_IDX.回数);
                        siiyto.コメント1_コメントコード = csv.GetField<int?>((int)SI_IY_IDX.コメント1_コメントコード) ?? 0;
                        siiyto.コメント1_文字データ = csv.GetField<string>((int)SI_IY_IDX.コメント1_文字データ);
                        siiyto.コメント2_コメントコード = csv.GetField<int?>((int)SI_IY_IDX.コメント2_コメントコード) ?? 0;
                        siiyto.コメント2_文字データ = csv.GetField<string>((int)SI_IY_IDX.コメント2_文字データ);
                        siiyto.コメント3_コメントコード = csv.GetField<int?>((int)SI_IY_IDX.コメント3_コメントコード) ?? 0;
                        siiyto.コメント3_文字データ = csv.GetField<string>((int)SI_IY_IDX.コメント3_文字データ);
                        for (int i = (int)SI_IY_IDX.X01日の情報; i <= (int)SI_IY_IDX.X31日の情報; i++)
                        {
                            var tmp = csv.GetField<int?>(i);
                            if (tmp.HasValue)
                            {
                                if (siiyto.XX日の情報 == null)
                                {
                                    siiyto.XX日の情報 = new Dictionary<int, int>();
                                }
                                var dateIdx = i - (int)SI_IY_IDX.X01日の情報;
                                siiyto.XX日の情報.Add(dateIdx, tmp.Value);
                            }
                        }

                        siiytocoList.Add(siiyto);
                    }
                    else if (reading && lineDef == レコード識別情報定数.特定器材)
                    {
                        var to = new TO();
                        to.診療識別 = csv.GetField<int?>((int)TO_IDX.診療識別);
                        to.負担区分 = csv.GetField<string>((int)TO_IDX.負担区分);
                        to.コード = (int)csv.GetField<int>((int)TO_IDX.特定器材コード);
                        to.数量 = csv.GetField<float?>((int)TO_IDX.使用量);
                        to.点数 = csv.GetField<int?>((int)TO_IDX.点数);
                        to.回数 = csv.GetField<int>((int)TO_IDX.回数);
                        to.単位コード = csv.GetField<int?>((int)TO_IDX.単位コード);
                        to.単価 = csv.GetField<float?>((int)TO_IDX.単価);
                        to.特定器材名称 = csv.GetField<string>((int)TO_IDX.特定器材名称);
                        to.商品名及び規格 = csv.GetField<string>((int)TO_IDX.商品名及び規格);
                        to.コメント1_コメントコード = csv.GetField<int?>((int)TO_IDX.コメント1_コメントコード) ?? 0;
                        to.コメント1_文字データ = csv.GetField<string>((int)TO_IDX.コメント1_文字データ);
                        to.コメント2_コメントコード = csv.GetField<int?>((int)TO_IDX.コメント2_コメントコード) ?? 0;
                        to.コメント2_文字データ = csv.GetField<string>((int)TO_IDX.コメント2_文字データ);
                        to.コメント3_コメントコード = csv.GetField<int?>((int)TO_IDX.コメント3_コメントコード) ?? 0;
                        to.コメント3_文字データ = csv.GetField<string>((int)TO_IDX.コメント3_文字データ);
                        for (int i = (int)TO_IDX.X01日の情報; i <= (int)TO_IDX.X31日の情報; i++)
                        {
                            var tmp = csv.GetField<int?>(i);
                            if (tmp.HasValue)
                            {
                                if (to.XX日の情報 == null)
                                {
                                    to.XX日の情報 = new Dictionary<int, int>();
                                }
                                var dateIdx = i - (int)TO_IDX.X01日の情報;
                                to.XX日の情報.Add(dateIdx, tmp.Value);
                            }
                        }
                        siiytocoList.Add(to);
                    }
                    else if (reading && lineDef == レコード識別情報定数.コメント)
                    {
                        var co = new CO()
                        {
                            診療識別 = csv.GetField<int?>((int)CO_IDX.診療識別),
                            負担区分 = csv.GetField<string>((int)CO_IDX.負担区分),
                            コメントコード = csv.GetField<int>((int)CO_IDX.コメントコード),
                            文字データ = csv.GetField<string>((int)CO_IDX.文字データ),
                        };
                        siiytocoList.Add(co);
                    }
                }
            };
            this.Read(filePath, readAction);
            return new Tuple<List<SY>, List<SIIYTOCO>>(syList, siiytocoList);
        }

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
            this.Read(filePath, readAction);
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
            this.Read(filePath, readAction);
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
            this.Read(filePath, readAction);
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
            this.Read(filePath, readAction);
            return dict;
        }

        private void Read(string filePath, Action<CsvReader> readAction)
        {
            using (var stream = new System.IO.StreamReader(filePath, Encoding.GetEncoding("Shift_JIS")))
            {
                var config = new CsvHelper.Configuration.CsvConfiguration()
                {
                    HasHeaderRecord = false,
                };

                using (var csv = new CsvReader(stream, config))
                {
                    readAction(csv);
                }
            }
        }

        /// <summary>次の患者レセプトを表示</summary>
        public RelayCommand NextPatientCommand
        {
            get
            {
                return _nextPatientCommand = _nextPatientCommand ??
                new RelayCommand(() =>
                {
                    if (CurrentPatient != null)
                    {
                        var id = PatientList.IndexOf(CurrentPatient);

                        if (id < (PatientList.Count - 1))
                        {
                            Patient pat = PatientList[id + 1];
                            CurrentPatient = pat;
                        }
                    }
                });
            }
        }
        private RelayCommand _nextPatientCommand;

        /// <summary>前の患者レセプトを表示</summary>
        public RelayCommand PreviousPatientCommand
        {
            get
            {
                return _previousPatientCommand = _previousPatientCommand ??
                new RelayCommand(() =>
                {
                    if (CurrentPatient != null)
                    {
                        var id = PatientList.IndexOf(CurrentPatient);

                        if (0 < id)
                        {
                            Patient pat = PatientList[id - 1];
                            CurrentPatient = pat;
                        }
                    }
                });
            }
        }
        private RelayCommand _previousPatientCommand;

        /// <summary></summary>
        /// <param name="orderByFunc"></param>
        private void SortPatientList(Func<Patient, int> orderByFunc)
        {
            if (this.PatientList == null) { return; }

            var list = this.PatientList.ToList();
            this.PatientList.Clear();
            foreach (var patient in list.OrderBy(orderByFunc))
            {
                this.PatientList.Add(patient);
            }
        }

        /// <summary>レセプト番号順で並べ替え</summary>
        public RelayCommand OrderByレセプト番号Command
        {
            get
            {
                return _orderByレセプト番号Command = _orderByレセプト番号Command ??
                new RelayCommand(() =>
                {
                    this.SortPatientList(x => x.RE.レセプト番号);
                });
            }
        }
        private RelayCommand _orderByレセプト番号Command;

        /// <summary>患者番号順で並べ替え</summary>
        public RelayCommand OrderBy患者番号Command
        {
            get
            {
                return _orderBy患者番号Command = _orderBy患者番号Command ??
                new RelayCommand(() =>
                {
                    this.SortPatientList(x => x.RE.患者番号);
                });
            }
        }
        private RelayCommand _orderBy患者番号Command;

        /// <summary>点数順で並べ替え</summary>
        public RelayCommand OrderBy合計点数Command
        {
            get
            {
                return _orderBy合計点数Command = _orderBy合計点数Command ??
                new RelayCommand(() =>
                {
                    // 高い順に並べたいので、合計点数のマイナスを返す。
                    this.SortPatientList(x => -((x.HO == null ? 0 : x.HO.合計点数) + (x.KO == null ? 0 : x.KO.合計点数)));
                });
            }
        }
        private RelayCommand _orderBy合計点数Command;
    }
}
