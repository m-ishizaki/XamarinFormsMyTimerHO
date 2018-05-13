# タイマー処理
このアプリのメイン機能である時間をカウントするためのタイマー処理を作成します。  
Xamarin.Forms では ```Xamarin.Forms.Device``` クラスの ```StartTimer``` メソッドでタイマーを実装できます。  
  
## 時間計測機能の作成
タイマーを定期的に実行すれば、その実行回数とタイマー間隔で経過時間がわかるという考えもあるかもしれません。あまり精度を必要としないアプリであれば、それで構いません。しかしタイマーの実行間隔の精度はそれほど高くありません。  
正確に時間を計りたい場合は、```System.Diagnostics.Stopwatch``` クラスが利用できます。  
ただし、```Stopwatch``` クラスにはタイマーのように定期的に何らかの処理を起動する機能はありませんので、これらのタイマーとストップウォッチを組み合わせて実装します。  
  
## タイマーの実行
```CountDownPageViewModel``` クラスに ```Stopwatch``` オブジェクトをフィールドとして保持します。  
次のコードを ```CountDownPageViewModel.cs``` に追加してください。
```cs
// タイマーカウント用のストップウォッチ
private System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
```

ストップウォッチとタイマーを起動します。  
次のコードを ```CountDownPageViewModel.cs``` のコンストラクタ ```public CountDownPageViewModel()``` に追加してください。
```cs
// ストップウォッチをスタート
_stopwatch.Start();
// カウントダウン更新タイマーをスタート
Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(33), OnTimerTick);
```

## タイマー間隔ごとの処理
タイマーを使ったコードでは一定間隔ごとに設定したメソッドが呼ばれます。  
一定間隔ごとに実行されるメソッドを作成します。  
次のコードを ```CountDownPageViewModel.cs``` に追加してください。
```cs
// 画面表示を更新
// カウントダウン中の残り時間の画面表示を更新する
// タイマー処理のたびに呼び出される
private void UpdateTime()
{
    var time = Math.Max(0, (TimerSettings.Instance.CountMilliseconds - _stopwatch.ElapsedMilliseconds));
    Console.WriteLine(time);
    Time = TimeSpan.FromMilliseconds(time);
}

// タイマー処理
// 毎回のタイマーイベントの処理
private bool OnTimerTick()
{
    // 残り時間を更新
    UpdateTime();
    // 時間が残っている場合、タイマーを続行
    if (Time.TotalMilliseconds > 0)
        return true;

    // 時間が経過しきった場合、一回だけ動かすタイマーをスタートし本タイマーは終了する
    Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(100),OnTimerEnd);
    return false;
}
```
```UpdateTime``` メソッド内で遷移前の View で設定したパラメータを参照していることも確認しておきましょう。 

## 設定時間経過時の処理
カウントダウンが終了した後に最後に一度だけ実行する処理を作成します。  
今回のアプリにはカウントダウン終了後に音声で通知する機能があります。音声を再生する処理は後の手順で作成しますので、ここでは空のメソッドだけ用意しておきます。  
次のコードを ```CountDownPageViewModel.cs``` に追加してください。
```cs
// 時間が経過しきった後の、一度だけ動かすタイマーの処理
private bool OnTimerEnd()
{
    // タイマー設定を確認し
    // テキストの読み上げ、または音声ファイルの再生を行う
    if (TimerSettings.Instance.UseSpeechText)
        TextToSpeech();
    else
        PlayAudio();
    return false;
}

// テキストの読み上げ
private async void TextToSpeech()
{
}

// 音声ファイルの再生
private void PlayAudio()
{
}
```

ここまでの手順で ```CountDownPageViewModel.cs```  のコードは次のようになります。
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyTimer
{
    // タイマーカウント画面の ViewModel
    class CountDownPageViewModel : BindableBase
    {
        // タイマーカウント用のストップウォッチ
        private System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

        // カウントの残り時間
        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }
            private set { SetProperty(ref _time, value); }
        }

        // 戻るボタンが押された
        public Command GoBackCommand { get; }

        // 画面を閉じ、カウント設定画面へ戻る
        private void GoBack()
        {
            // 画面を「戻る」メッセージを送信
            MessagingCenter.Send(this, "GoBack");
        }

        // コンストラクタ
        public CountDownPageViewModel()
        {
            // コマンドの設定
            // readonly プロパティの初期化は、コンストラクタ内でも行える
            GoBackCommand = new Command(GoBack);

            // ストップウォッチをスタート
            _stopwatch.Start();
            // カウントダウン更新タイマーをスタート
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(33), OnTimerTick);
        }

        // 画面表示を更新
        // カウントダウン中の残り時間の画面表示を更新する
        // タイマー処理のたびに呼び出される
        private void UpdateTime()
        {
            var time = Math.Max(0, (TimerSettings.Instance.CountMilliseconds - _stopwatch.ElapsedMilliseconds));
            Console.WriteLine(time);
            Time = TimeSpan.FromMilliseconds(time);
        }

        // タイマー処理
        // 毎回のタイマーイベントの処理
        private bool OnTimerTick()
        {
            // 残り時間を更新
            UpdateTime();
            // 時間が残っている場合、タイマーを続行
            if (Time.TotalMilliseconds > 0)
                return true;

            // 時間が経過しきった場合、一回だけ動かすタイマーをスタートし本タイマーは終了する
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(100), OnTimerEnd);
            return false;
        }

        // 時間が経過しきった後の、一度だけ動かすタイマーの処理
        private bool OnTimerEnd()
        {
            // タイマー設定を確認し
            // テキストの読み上げ、または音声ファイルの再生を行う
            if (TimerSettings.Instance.UseSpeechText)
                TextToSpeech();
            else
                PlayAudio();
            return false;
        }

        // テキストの読み上げ
        private async void TextToSpeech()
        {
        }

        // 音声ファイルの再生
        private void PlayAudio()
        {
        }

    }
}
```

[< 前ページ](./textbook10.md) | [次ページ >](./textbook12.md)  