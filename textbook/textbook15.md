# AudioPlayer
タイマー設定時間の経過を音声で知らせる機能を実装します。  
音声ファイルを再生する機能は、プラットフォーム毎の機能になり Xaamrin.Forms の機能にはありません。前述の DependencyService を利用することで自身で実装することもできますが、今回は TextToSpeech と同様に NuGet からライブラリを取得し利用します。

## AudioPlayer の追加
NuGet パッケージの追加は [TextToSpeech](./textbook14.md) で学んでいます。思い出しながら ```Xam.Plugin.SimpleAudioPlayer``` を追加してください。 

## AudioPlayer の利用
AudioPlayer を利用するには、```Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer``` クラスの static プロパティ ```Current``` で取得できるオブジェクトの ```Load``` メソッドと ```Play``` メソッドをメソッドを実行するだけです。再生する音声データは```Load```メソッドの引数で設定します。  
今回再生する音声ファイルは [背景画像の表示](./textbook03.md) の手順ですでに追加しています。

```CountDownPageViewModel``` クラスの ```PlayAudio``` メソッドを次のコードで上書きしてください。
```cs
// 音声ファイルの再生
private void PlayAudio()
{
    try
    {
        var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
        var assembly = typeof(App).Assembly;
        var name = "MyTimer.Resources.voice.m4a";
        // name 確認
        if (!IsContainsResource(assembly, name)) return;
        using (var stream = assembly.GetManifestResourceStream(name))
            player.Load(stream);
        player.Play();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        throw;
    }
}
```
**※ファイル名の指定などでミスをしやすいためエラーハンドリングなどを行っているためコードが長くなっています。コードは後日お時間のある時にゆっくり確認してみてください**

音声ファイル名の指定は特に間違えやすくエラーもわかりにくいため専用のチェックメソッドを作成します。  
次のコードを ```CountDownPageViewModel``` クラスに追加してください。
```cs
// 音声ファイルのリソース名指定に誤りがないかチェックをする
private bool IsContainsResource(System.Reflection.Assembly assembly, string name)
{
    if (assembly.GetManifestResourceNames().Contains(name)) return true;
    Console.WriteLine($"name : {name} はリソースに存在しません。");
    Console.WriteLine("リソースに含まれるファイル：");
    foreach (var resoureName in assembly.GetManifestResourceNames())
        Console.WriteLine(resoureName);
    return false;
}
```

ここまでの手順で ```CountDownPageViewModel.cs``` は次のようになっています。
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

            // 画面をスリープしない設定
            DependencyService.Get<IKeepScreenOn>().Set(true);
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
            await Plugin.TextToSpeech.CrossTextToSpeech.Current.Speak(TimerSettings.Instance.SpeechText, volume: 0.5f);
        }

        // 音声ファイルの再生
        private void PlayAudio()
        {
            try
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                var assembly = typeof(App).Assembly;
                var name = "MyTimer.Resources.voice.m4a";
                // name 確認
                if (!IsContainsResource(assembly, name)) return;
                using (var stream = assembly.GetManifestResourceStream(name))
                    player.Load(stream);
                player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // 音声ファイルのリソース名指定に誤りがないかチェックをする
        private bool IsContainsResource(System.Reflection.Assembly assembly, string name)
        {
            if (assembly.GetManifestResourceNames().Contains(name)) return true;
            Console.WriteLine($"name : {name} はリソースに存在しません。");
            Console.WriteLine("リソースに含まれるファイル：");
            foreach (var resoureName in assembly.GetManifestResourceNames())
                Console.WriteLine(resoureName);
            return false;
        }

    }
}
```

## 実行 - 完成
これでアプリは完成です。  
実行してタイマーアプリとして動作することを確認しましょう！

[< 前ページ](./textbook14.md) | [次ページ >](./textbook16.md)  