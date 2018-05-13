using System;
using System.Collections.Generic;
using System.Text;

namespace MyTimer
{
    // アラートダイアログ表示メッセージのパラメーター
    class AlertParameter
    {
        // 表示するアラートダイアログのタイトルを取得または設定する
        public string Title { get; set; }
        // 表示するアラートダイアログのメッセージを取得または設定する
        public string Message { get; set; }
        // 表示するアラートダイアログの OK ボタンのテキストを取得または設定する
        public string Accept { get; set; }
        // 表示するアラートダイアログのキャンセルボタンのテキストを取得または設定する
        public string Cancel { get; set; }
        // 表示するアラートダイアログの選択時に呼ばれる処理を取得または設定する
        public Action<bool> Action { get; set; }
    }
}