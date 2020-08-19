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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenReceiptViewer
{
    public static class レコード識別情報定数
    {
        /// <summary></summary>
        public const string 医療機関情報 = "IR";

        /// <summary></summary>
        public const string レセプト共通 = "RE";

        /// <summary></summary>
        public const string 保険者 = "HO";

        /// <summary></summary>
        public const string 公費 = "KO";

        /// <summary></summary>
        public const string 資格確認 = "SN";

        /// <summary></summary>
        public const string 受診日 = "JD";

        /// <summary></summary>
        public const string 窓口負担額 = "MF";

        /// <summary></summary>
        public const string 包括評価対象外理由 = "GR";

        /// <summary></summary>
        public const string 傷病名 = "SY";

        /// <summary></summary>
        public const string 診療行為 = "SI";

        /// <summary></summary>
        public const string 医薬品 = "IY";

        /// <summary></summary>
        public const string 特定器材 = "TO";

        /// <summary></summary>
        public const string コメント = "CO";

        /// <summary></summary>
        public const string 症状詳記 = "SJ";

        /// <summary></summary>
        public const string 診療報酬請求書 = "GO";
    }

    /// <summary></summary>
    public static class Define
    {
        /// <summary></summary>
        public const int 公費最大件数 = 4;

        /// <summary></summary>
        public const int 未コード化傷病コード = 999;
    }
}
