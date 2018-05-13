using System;
using System.Collections.Generic;
using System.Text;

namespace MyTimer
{
    // タイマー設定保持クラス
    class TimerSettings
    {
        // アプリ内で単一のインスタンスを取得する
        // タイマー設定はアプリで一つだけ保持する
        public static TimerSettings Instance { get; } = new TimerSettings();

        // コンストラクタ
        // 外部でインスタンス化できないよう private にする
        private TimerSettings() {; }

        // カウントする時間を取得または設定する
        public double CountMilliseconds { get; set; }
        // カウント終了時に読み上げるテキストを取得または設定する
        public string SpeechText { get; set; }
        // カント終了時にテキストを読み上げるか否かを取得または設定する
        public bool UseSpeechText { get; set; }
    }
}
