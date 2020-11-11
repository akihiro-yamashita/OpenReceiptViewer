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
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.IO;

namespace OpenReceiptViewer
{
    public class ViewerViewModel : NotifyPropertyChanged
    {
        /// <summary>医療機関情報レコード</summary>
        public IR IR { get; set; }

        /// <summary>診療報酬請求書レコード</summary>
        public GO GO { get; set; }

        /// <summary>レセプトリスト</summary>
        public ObservableCollection<Receipt> ReceiptList { get; set; }

        /// <summary>条件使用時のReceiptList退避用</summary>
        public List<Receipt> ReceiptListOriginal { get; set; }

        /// <summary>選択中のレセプト</summary>
        public Receipt CurrentReceipt
        {
            get
            {
                return this._currentReceipt;
            }
            set
            {
                this._currentReceipt = value;
                OnPropertyChanged("CurrentReceipt");
            }
        }
        private Receipt _currentReceipt;

        public string ReceiptFilePath = string.Empty;

        public bool IsNumberOnlyカルテ番号 { get; private set; } = true;

        public ViewerViewModel()
        {
            this.IR = new IR();
            this.GO = new GO();
            this.ReceiptList = new ObservableCollection<Receipt>();
            this.ReceiptListOriginal = null;
            this.MasterRootDiretoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Master");
        }

        /// <summary></summary>
        public RelayCommand OpenCommand
        {
            get
            {
                return _openCommand = _openCommand ??
                new RelayCommand(() =>
                {
                    try
                    {
                        Read(ReceiptFilePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    // TODO: Read内で都度マスター読んでもよい？
                    foreach (var x in this.ReceiptList)
                    {
                        var masterVersion = EnumUtil.CalcMasterVersion(x.RE.診療年月);
                        if (InitializedMasterVersions.Contains(masterVersion) == false)
                        {
                            // 該当診療年月に対応したマスターバージョンを読み込む。
                            InitializeMasterConverter(masterVersion);
                            InitializedMasterVersions.Add(masterVersion);
                        }
                    }

                    SelectFirstReceipt();
                });
            }
        }
        private RelayCommand _openCommand;

        private void Read(string filePath)
        {
            Action<CsvReader> readAction = csv =>
            {
                var patient = (Receipt)null;

                Action add = () =>
                {
                    // 公費の件数調整
                    while (Define.公費最大件数 < patient.KOList.Count)
                    {
                        Debug.Assert(false, string.Format("レセプトの仕様上、公費は最大{0}件までです。", Define.公費最大件数));
                        patient.KOList.RemoveAt(patient.KOList.Count - 1);
                    }
                    while (patient.KOList.Count < Define.公費最大件数)
                    {
                        // KOListをindex指定でバインドしているため、空データ入れた方が都合が良い。
                        patient.KOList.Add(null);
                    }

                    this.ReceiptList.Add(patient);
                    patient = null;
                };

                while (csv.Read())
                {
                    var lineDef = csv.GetField<string>(0);
                    if (lineDef == レコード識別情報定数.医療機関情報)
                    {
                        this.IR.審査支払機関 = (審査支払機関)csv.GetField<int>((int)IR_IDX.審査支払機関);
                        this.IR.都道府県 = csv.GetField<int>((int)IR_IDX.都道府県);
                        this.IR.点数表 = csv.GetField<int>((int)IR_IDX.点数表);
                        this.IR.医療機関コード = csv.GetField<int>((int)IR_IDX.医療機関コード);
                        this.IR.予備 = csv.GetField<int?>((int)IR_IDX.予備);
                        this.IR.医療機関名称 = csv.GetField<string>((int)IR_IDX.医療機関名称);
                        this.IR.請求年月 = csv.GetField<int>((int)IR_IDX.請求年月);
                        this.IR.マルチボリューム識別子 = csv.GetField<int>((int)IR_IDX.マルチボリューム識別子);
                        this.IR.電話番号 = csv.GetField<string>((int)IR_IDX.電話番号);
                        this.IR.医療機関名称 = csv.GetField<string>((int)IR_IDX.医療機関名称);
                    }
                    else if (lineDef == レコード識別情報定数.診療報酬請求書)
                    {
                        this.GO.総件数 = csv.GetField<int>((int)GO_IDX.総件数);
                        this.GO.総合計点数 = csv.GetField<int>((int)GO_IDX.総合計点数);
                        this.GO.マルチボリューム識別子 = csv.GetField<int>((int)GO_IDX.マルチボリューム識別子);
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
                            カルテ番号 = csv.GetField<string>((int)RE_IDX.カルテ番号等),
                        };
                        if (csv.TryGetField<int>((int)RE_IDX.入院年月日, out int tmp入院年月日))
                        {
                            // 入院レセプトのみ
                            re.入院年月日 = tmp入院年月日;
                        }
                        if (csv.TryGetField<string>((int)RE_IDX.カタカナ, out string tmpカタカナ))
                        {
                            // H30年4月以降
                            re.カタカナ = tmpカタカナ;
                        }
                        if (csv.TryGetField<string>((int)RE_IDX.患者の状態, out string tmp患者の状態))
                        {
                            // H30年4月以降
                            re.患者の状態 = tmp患者の状態;
                        }

                        // 1件でも数値変換不可能なカルテ番号が来たらIsNumberOnlyカルテ番号をfalseに。
                        if (IsNumberOnlyカルテ番号 && Int32.TryParse(re.カルテ番号, out int _) == false)
                        {
                            IsNumberOnlyカルテ番号 = false;
                        }

                        patient = new Receipt()
                        {
                            KOList = new List<KO>(),
                            SIIYTOCOList = new List<SIIYTOCO>(),
                            SYList = new List<SY>(),
                        };
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
                            patient.KOList.Add(ko);
                        }
                        else
                        {
                            Debug.Assert(false, "公費レコードの順番が不正です。");
                        }
                    }
                    else if (lineDef == レコード識別情報定数.資格確認)
                    {
                    }
                    else if (lineDef == レコード識別情報定数.受診日)
                    {
                    }
                    else if (lineDef == レコード識別情報定数.窓口負担額)
                    {
                    }
                    else if (lineDef == レコード識別情報定数.包括評価対象外理由)
                    {
                    }
                    else if (lineDef == レコード識別情報定数.傷病名)
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
                        patient.SYList.Add(sy);
                    }
                    else if (lineDef == レコード識別情報定数.診療行為 || lineDef == レコード識別情報定数.医薬品)
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

                        patient.SIIYTOCOList.Add(siiyto);
                    }
                    else if (lineDef == レコード識別情報定数.特定器材)
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
                        patient.SIIYTOCOList.Add(to);
                    }
                    else if (lineDef == レコード識別情報定数.コメント)
                    {
                        var co = new CO()
                        {
                            診療識別 = csv.GetField<int?>((int)CO_IDX.診療識別),
                            負担区分 = csv.GetField<string>((int)CO_IDX.負担区分),
                            コメントコード = csv.GetField<int>((int)CO_IDX.コメントコード),
                            文字データ = csv.GetField<string>((int)CO_IDX.文字データ),
                        };
                        patient.SIIYTOCOList.Add(co);
                    }
                    else if (lineDef == レコード識別情報定数.症状詳記)
                    {
                    }

                }

                add();  // 最後の患者を追加する。
            };
            CSVUtil.Read(filePath, readAction);
        }

        #region 操作

        private void SelectFirstReceipt()
        {
            if (0 < this.ReceiptList.Count)
            {
                this.CurrentReceipt = this.ReceiptList[0];
            }
        }
        /// <summary>次のレセプトを表示</summary>
        public RelayCommand NextReceiptCommand
        {
            get
            {
                return _nextReceiptCommand = _nextReceiptCommand ??
                new RelayCommand(() =>
                {
                    if (CurrentReceipt != null)
                    {
                        var id = ReceiptList.IndexOf(CurrentReceipt);

                        if (id < (ReceiptList.Count - 1))
                        {
                            var receipt = ReceiptList[id + 1];
                            CurrentReceipt = receipt;
                        }
                    }
                });
            }
        }
        private RelayCommand _nextReceiptCommand;

        /// <summary>前のレセプトを表示</summary>
        public RelayCommand PreviousReceiptCommand
        {
            get
            {
                return _previousReceiptCommand = _previousReceiptCommand ??
                new RelayCommand(() =>
                {
                    if (CurrentReceipt != null)
                    {
                        var id = ReceiptList.IndexOf(CurrentReceipt);

                        if (0 < id)
                        {
                            var receipt = ReceiptList[id - 1];
                            CurrentReceipt = receipt;
                        }
                    }
                });
            }
        }
        private RelayCommand _previousReceiptCommand;

        #endregion

        #region 検索

        /// <summary>カルテ番号検索</summary>
        public RelayCommand NumberSearchCommand
        {
            get
            {
                return _numberSearchCommand = _numberSearchCommand ??
                new RelayCommand(() =>
                {
                    if (this.ReceiptList.Count == 0) { return; }

                    var window = new SearchWindow();
                    window.Title = "カルテ番号検索";
                    window.Label.Content = "カルテ番号";
                    window.Owner = Application.Current.MainWindow;

                    var dialogResult = window.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        var input = ((SearchWindowViewModel)window.DataContext).Input;
                        if (input == null) { return; }
                        var inputTrim = input.Trim();
                        if (0 < inputTrim.Length)
                        {
                            var result = ReceiptList.FirstOrDefault(x => x.RE.カルテ番号 == inputTrim, CurrentReceipt);
                            if (result == null)
                            {
                                MessageBox.Show("指定されたカルテ番号は見つかりませんでした。");
                            }
                            else
                            {
                                this.CurrentReceipt = result;
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
                    if (this.ReceiptList.Count == 0) { return; }

                    var window = new SearchWindow();
                    window.Title = "氏名検索";
                    window.Label.Content = "氏名";
                    var dialogResult = window.ShowDialog();
                    if (dialogResult.HasValue && dialogResult.Value)
                    {
                        var input = ((SearchWindowViewModel)window.DataContext).Input;
                        if (!string.IsNullOrEmpty(input))
                        {
                            var result = ReceiptList.FirstOrDefault(x => x.RE.氏名.Contains(input), CurrentReceipt);
                            if (result == null)
                            {
                                MessageBox.Show("指定された患者は見つかりませんでした。");
                            }
                            else
                            {
                                this.CurrentReceipt = result;
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
                    if (this.ReceiptListOriginal == null)
                    {
                        return;
                    }

                    this.ReceiptList.Clear();
                    foreach (var re in this.ReceiptListOriginal)
                    {
                        this.ReceiptList.Add(re);
                    }
                    this.ReceiptListOriginal = null;

                    SelectFirstReceipt();
                });
            }
        }
        private RelayCommand _clearFilterCommand;

        private void FilterAction(string masterSubDiretoryPath, string レコード識別情報)
        {
            if (this.ReceiptListOriginal == null)
            {
                // 条件がかかっていない時は表示中レセプトのカウントをチェック
                if (this.ReceiptList.Count == 0) { return; }
            }
            else
            {
                //// 条件がかかっている時は退避レセプトのカウントをチェック
                //if (this.ReceiptListOriginal.Count == 0) { return; }

                // 一旦既存条件クリア
                this.ClearFilterCommand.Execute(null);
            }

            var レコード識別名称 = string.Empty;
            var receiptIdField = 0;
            var fileName = (string)null;
            if (レコード識別情報 == レコード識別情報定数.診療行為)
            {
                レコード識別名称 = "診療行為";
                receiptIdField = (int)SI_IY_IDX.診療行為または医薬品コード;
                fileName = "s.csv";
            }
            else if (レコード識別情報 == レコード識別情報定数.医薬品)
            {
                レコード識別名称 = "医薬品";
                receiptIdField = (int)SI_IY_IDX.診療行為または医薬品コード;
                fileName = "y.csv";
            }
            else if (レコード識別情報 == レコード識別情報定数.特定器材)
            {
                レコード識別名称 = "特定器材";
                receiptIdField = (int)TO_IDX.特定器材コード;
                fileName = "t.csv";
            }
            else if (レコード識別情報 == レコード識別情報定数.コメント)
            {
                レコード識別名称 = "コメント";

                // コメントマスターファイルはここでは見ない。
                // 代わりにコメントConverterの文字列を見る。
            }
            else
            {
                Debug.Assert(false);
                return;
            }

            // 条件ダイアログ
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
            var inputUpper = input.ToUpper();  // 大文字同士で比較

            // 条件合致のレセプト番号
            var dict = new Dictionary<int, int>();

            if (レコード識別情報 == レコード識別情報定数.コメント)
            {
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

                            var コメントコード = csv.GetField<int>((int)CO_IDX.コメントコード);
                            var 文字データ = csv.GetField<string>((int)CO_IDX.文字データ);
                            var tmp = コメントConverter.Instance.Convert(コメントコード, 文字データ, null);
                            if (tmp.StartsWith("※"))
                            {
                                // コメントConverterが勝手に付けている自由入力マーク「※」を消して比較
                                tmp = tmp.Substring(1);
                            }
                            // 「退　院　～～」のようなコメントがあるので空白削除版も一応比較
                            var tmp2 = tmp.Replace("　", "");

                            if (tmp.StartsWith(inputUpper))
                            {
                                dict.Add(currentReceiptNo, currentReceiptNo);
                            }
                            else if (tmp2.StartsWith(inputUpper))
                            {
                                dict.Add(currentReceiptNo, currentReceiptNo);
                            }
                        }
                    }
                };
                CSVUtil.Read(ReceiptFilePath, readAction);
            }
            else  // レコード識別情報 in 診療行為, 医薬品, 特定器材
            {
                var masterFilePath = Path.Combine(MasterRootDiretoryPath, masterSubDiretoryPath, fileName);
                var masterIds = new Dictionary<int, int>();

                // 対象マスターを探す。
                Action<CsvReader> sReadAction = csv =>
                {
                    while (csv.Read())
                    {
                        var id = csv.GetField<int>((int)MASTER_S_Y_T_IDX.コード);
                        var name = csv.GetField<string>((int)MASTER_S_Y_T_IDX.名称).ToUpper();
                        if (name.StartsWith(inputUpper))
                        {
                            masterIds.Add(id, id);
                        }
                    }
                };
                CSVUtil.Read(masterFilePath, sReadAction);
                if (masterIds.Count == 0)
                {
                    MessageBox.Show("「" + input + "」から始まる" + レコード識別名称 + "マスターがありません。");
                    return;
                }

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

                            var id = csv.GetField<int>(receiptIdField);
                            if (masterIds.ContainsKey(id))
                            {
                                dict.Add(currentReceiptNo, currentReceiptNo);
                            }
                        }
                    }
                };
                CSVUtil.Read(ReceiptFilePath, readAction);
            }

            // オリジナル保存
            this.ReceiptListOriginal = new List<Receipt>(this.ReceiptList);

            // 条件一致のみ再追加
            this.ReceiptList.Clear();
            foreach (var patient in this.ReceiptListOriginal)
            {
                if (dict.ContainsKey(patient.RE.レセプト番号))
                {
                    this.ReceiptList.Add(patient);
                }
            }

            SelectFirstReceipt();
        }

        private MasterVersion? GetFirstMasterVersion()
        {
            var first = (Receipt)null;

            // 条件がかかっていて退避中ならReceiptListOriginalを見る。
            if (this.ReceiptListOriginal != null && 0 < this.ReceiptListOriginal.Count)
            {
                first = this.ReceiptListOriginal[0];
            }
            else if (this.ReceiptList != null && 0 < this.ReceiptList.Count)
            {
                first = this.ReceiptList[0];
            }
            else
            {
                return null;
            }

            return EnumUtil.CalcMasterVersion(first.RE.診療年月);
        }

        /// <summary>診療行為条件</summary>
        public RelayCommand<string> 診療行為FilterCommand
        {
            get
            {
                return _診療行為FilterCommand = _診療行為FilterCommand ??
                new RelayCommand<string>(masterSubDiretoryPath =>
                {
                    // レセプト1件目の診療年月の対応マスターバージョンから検索対象を読み込む。
                    // 月遅れレセプトが混在されても正しく検索できないが、この仕様で。
                    var masterVersion = GetFirstMasterVersion();
                    if (masterVersion.HasValue)
                    {
                        this.FilterAction(EnumUtil.GetMasterSubDiretoryName(masterVersion.Value), レコード識別情報定数.診療行為);
                    }
                });
            }
        }
        private RelayCommand<string> _診療行為FilterCommand;

        /// <summary>医薬品条件</summary>
        public RelayCommand<string> 医薬品FilterCommand
        {
            get
            {
                return _医薬品FilterCommand = _医薬品FilterCommand ??
                new RelayCommand<string>(masterSubDiretoryPath =>
                {
                    var masterVersion = GetFirstMasterVersion();
                    if (masterVersion.HasValue)
                    {
                        this.FilterAction(EnumUtil.GetMasterSubDiretoryName(masterVersion.Value), レコード識別情報定数.医薬品);
                    }
                });
            }
        }
        private RelayCommand<string> _医薬品FilterCommand;

        /// <summary>特定器材条件</summary>
        public RelayCommand<string> 特定器材FilterCommand
        {
            get
            {
                return _特定器材FilterCommand = _特定器材FilterCommand ??
                new RelayCommand<string>(masterSubDiretoryPath =>
                {
                    var masterVersion = GetFirstMasterVersion();
                    if (masterVersion.HasValue)
                    {
                        this.FilterAction(EnumUtil.GetMasterSubDiretoryName(masterVersion.Value), レコード識別情報定数.特定器材);
                    }
                });
            }
        }
        private RelayCommand<string> _特定器材FilterCommand;

        /// <summary>コメント条件</summary>
        public RelayCommand<string> コメントFilterCommand
        {
            get
            {
                return _コメントFilterCommand = _コメントFilterCommand ??
                new RelayCommand<string>(masterSubDiretoryPath =>
                {
                    var masterVersion = GetFirstMasterVersion();
                    if (masterVersion.HasValue)
                    {
                        this.FilterAction(EnumUtil.GetMasterSubDiretoryName(masterVersion.Value), レコード識別情報定数.コメント);
                    }
                });
            }
        }
        private RelayCommand<string> _コメントFilterCommand;

        #endregion

        #region 並べ替え

        /// <summary></summary>
        /// <param name="orderByFunc"></param>
        private void SortReceiptList<TOrder>(Func<Receipt, TOrder> orderByFunc)
        {
            if (this.ReceiptList == null) { return; }

            var list = this.ReceiptList.ToList();
            this.ReceiptList.Clear();
            foreach (var receipt in list.OrderBy(orderByFunc))
            {
                this.ReceiptList.Add(receipt);
            }

            SelectFirstReceipt();
        }

        /// <summary>レセプト番号順で並べ替え</summary>
        public RelayCommand OrderByレセプト番号Command
        {
            get
            {
                return _orderByレセプト番号Command = _orderByレセプト番号Command ??
                new RelayCommand(() =>
                {
                    this.SortReceiptList(x => x.RE.レセプト番号);
                });
            }
        }
        private RelayCommand _orderByレセプト番号Command;

        /// <summary>カルテ番号順で並べ替え</summary>
        public RelayCommand OrderByカルテ番号Command
        {
            get
            {
                return _orderByカルテ番号Command = _orderByカルテ番号Command ??
                new RelayCommand(() =>
                {
                    if (IsNumberOnlyカルテ番号)
                    {
                        this.SortReceiptList(x => Int32.Parse(x.RE.カルテ番号));
                    }
                    else
                    {
                        this.SortReceiptList(x => x.RE.カルテ番号);
                    }
                });
            }
        }
        private RelayCommand _orderByカルテ番号Command;

        /// <summary>点数順で並べ替え</summary>
        public RelayCommand OrderBy合計点数Command
        {
            get
            {
                return _orderBy合計点数Command = _orderBy合計点数Command ??
                new RelayCommand(() =>
                {
                    // 高い順に並べたいので、合計点数のマイナスを返す。
                    this.SortReceiptList(x => -(x.HO == null ? 0 : x.HO.合計点数));
                });
            }
        }
        private RelayCommand _orderBy合計点数Command;

        #endregion

        #region マスター関連

        public string MasterRootDiretoryPath { get; private set; }

        private void InitializeMasterConverter(MasterVersion masterVersion)
        {
            傷病名Converter.Instance.傷病名Dict = this.Read傷病名(masterVersion);
            傷病名Converter.Instance.修飾語Dict = this.Read修飾語(masterVersion);
            コメントConverter.Instance.コメントDict = this.Readコメント(masterVersion);

            var 診療行為List = this.Read診療行為(masterVersion);
            DictConverter.診療行為Instance((int)masterVersion).Dict = 診療行為List.ToDictionary(x => x.Id, x => x.名称);
            DictConverter.診療行為単位Instance.Dict = 診療行為List.ToDictionary(x => x.Id, x => x.単位);
            var 医薬品List = this.Read医薬品(masterVersion);
            DictConverter.医薬品Instance((int)masterVersion).Dict = 医薬品List.ToDictionary(x => x.Id, x => x.名称);
            DictConverter.医薬品単位Instance.Dict = 医薬品List.ToDictionary(x => x.Id, x => x.単位);
            var 特定器材List = this.Read特定器材(masterVersion);
            DictConverter.特定器材Instance((int)masterVersion).Dict = 特定器材List.ToDictionary(x => x.Id, x => x.名称);
            DictConverter.特定器材単位Instance.Dict = 特定器材List.ToDictionary(x => x.Id, x => x.単位);
        }
        private static List<MasterVersion> InitializedMasterVersions = new List<MasterVersion>();

        private Dictionary<int, string> Read傷病名(MasterVersion masterVersion)
        {
            var filePath = Path.Combine(MasterRootDiretoryPath, EnumUtil.GetMasterSubDiretoryName(masterVersion), "b.csv");

            var dict = new Dictionary<int, string>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var id = csv.GetField<int>((int)MASTER_B_IDX.傷病名コード);
                    var name = csv.GetField<string>((int)MASTER_B_IDX.傷病名基本名称);
                    dict.Add(id, name);
                }
            };
            CSVUtil.Read(filePath, readAction);
            return dict;
        }

        private Dictionary<int, string> Read修飾語(MasterVersion masterVersion)
        {
            var filePath = Path.Combine(MasterRootDiretoryPath, EnumUtil.GetMasterSubDiretoryName(masterVersion), "z.csv");

            var dict = new Dictionary<int, string>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var id = csv.GetField<int>((int)MASTER_Z_IDX.修飾語コード);
                    var name = csv.GetField<string>((int)MASTER_Z_IDX.修飾語名称);
                    dict.Add(id, name);
                }
            };
            CSVUtil.Read(filePath, readAction);
            return dict;
        }

        private List<名称単位マスター> Read名称単位マスター(MasterVersion masterVersion, string fileName)
        {
            var filePath = Path.Combine(MasterRootDiretoryPath, EnumUtil.GetMasterSubDiretoryName(masterVersion), fileName);

            var list = new List<名称単位マスター>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var id = csv.GetField<int>((int)MASTER_S_Y_T_IDX.コード);
                    var 名称 = csv.GetField<string>((int)MASTER_S_Y_T_IDX.名称);
                    var 単位 = csv.GetField<string>((int)MASTER_S_Y_T_IDX.単位);
                    list.Add(new 名称単位マスター() { Id = id, 名称 = 名称, 単位 = 単位 });
                }
            };
            CSVUtil.Read(filePath, readAction);
            return list;
        }

        private List<名称単位マスター> Read診療行為(MasterVersion masterVersion)
        {
            return Read名称単位マスター(masterVersion, "s.csv");
        }

        private List<名称単位マスター> Read医薬品(MasterVersion masterVersion)
        {
            return Read名称単位マスター(masterVersion, "y.csv");
        }

        private List<名称単位マスター> Read特定器材(MasterVersion masterVersion)
        {
            return Read名称単位マスター(masterVersion, "t.csv");
        }

        private Dictionary<int, コメントマスター> Readコメント(MasterVersion masterVersion)
        {
            var filePath = Path.Combine(MasterRootDiretoryPath, EnumUtil.GetMasterSubDiretoryName(masterVersion), "c.csv");

            var dict = new Dictionary<int, コメントマスター>();
            Action<CsvReader> readAction = csv =>
            {
                while (csv.Read())
                {
                    var x = new コメントマスター();
                    x.区分 = csv.GetField<int>((int)MASTER_C_IDX.区分);
                    x.パターン = csv.GetField<int>((int)MASTER_C_IDX.パターン);
                    x.一連番号 = csv.GetField<int>((int)MASTER_C_IDX.一連番号);
                    x.漢字名称 = csv.GetField<string>((int)MASTER_C_IDX.漢字名称);
                    x.カラム位置桁数 = new List<Tuple<int, int>>();

                    // 4回まである。
                    var カラム位置 = csv.GetField<int>((int)MASTER_C_IDX.カラム1位置);
                    var カラム桁数 = csv.GetField<int>((int)MASTER_C_IDX.カラム1桁数);
                    if (0 < カラム桁数)
                    {
                        x.カラム位置桁数.Add(new Tuple<int, int>(カラム位置, カラム桁数));
                    }
                    カラム位置 = csv.GetField<int>((int)MASTER_C_IDX.カラム2位置);
                    カラム桁数 = csv.GetField<int>((int)MASTER_C_IDX.カラム2桁数);
                    if (0 < カラム桁数)
                    {
                        x.カラム位置桁数.Add(new Tuple<int, int>(カラム位置, カラム桁数));
                    }
                    カラム位置 = csv.GetField<int>((int)MASTER_C_IDX.カラム3位置);
                    カラム桁数 = csv.GetField<int>((int)MASTER_C_IDX.カラム3桁数);
                    if (0 < カラム桁数)
                    {
                        x.カラム位置桁数.Add(new Tuple<int, int>(カラム位置, カラム桁数));
                    }
                    カラム位置 = csv.GetField<int>((int)MASTER_C_IDX.カラム4位置);
                    カラム桁数 = csv.GetField<int>((int)MASTER_C_IDX.カラム4桁数);
                    if (0 < カラム桁数)
                    {
                        x.カラム位置桁数.Add(new Tuple<int, int>(カラム位置, カラム桁数));
                    }

#if DEBUG
                    var コメントコード = csv.GetField<int>((int)MASTER_C_IDX.コメントコード);
                    Debug.Assert(コメントコード == x.コメントコード);
#endif

                    dict.Add(x.コメントコード, x);
                }
            };
            CSVUtil.Read(filePath, readAction);
            return dict;
        }

        #endregion
    }
}
