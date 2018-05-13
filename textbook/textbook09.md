# 画面遷移
画面遷移はプラットフォーム毎に様々なスタイルがあります。  
Xamarin.Forms では、新しいページを作り今のページの上に重寝て表示するイメージの実装になっています。  
  
## 画面遷移のコード
画面遷移をするには ```Navigation``` オブジェクトの ```Push～``` メソッドを実行します。
その際、```Push～``` メソッドの引数には次のページのインスタンスを設定ます。  
```Push～``` メソッドは複数ありますが、今回は ```PushModalAsync``` を使用します。

コードは次のようになります。
```cs
this.Navigation.PushModalAsync(new CountDownPage());
```

次のコードを ```MainPage.cs``` の ```StartTimer``` メソッドの中に追加します。
```cs
// タイマー画面へ遷移する
this.Navigation.PushModalAsync(new CountDownPage());
```

```Start``` メソッド全体は次のようになります。
```cs
// タイマーをスタートする
private void StartTimer<T>(T sender)
{
    // タイマー画面へ遷移する
    this.Navigation.PushModalAsync(new CountDownPage());
}
```

## 画面遷移のパラメーター
今回のアプリは 1 ページ目がタイマーの設定、2 ページ目がタイマーのカウント (実行) 画面になっています。  
1 ページ目で行った設定を 2 ページ目に渡す必要があります。設定をページ間で共有する方法はいくつかありますが、今回シンプルなアプリなのでコードの短い static インスタンスの共有で実装します。  

## パラメータ設定クラスの作成
```MyTimer``` プロジェクト(共有コードプロジェクト)に ```TimerSettings``` クラスを作成します。  
クラスの作成方法は [背景画像の表示](./textbook03.md) で学んでいます。思い出しながら作成してください。  

パラメーター設定のクラスは、画面間で共有するプロパティとして持ったプレーンなクラスです。

今回は次のコードで ```TimerSettings.cs``` ファイルの内容を上書きしてください。
```cs
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
```

## パラメーターの設定
メインページでのパラメータの設定を作成します。  
```MainPageViewModel.cs``` の ```Start``` メソッドの先頭に次のコードを追加します。
```cs
// タイマー設定の保存
var settings = TimerSettings.Instance;
settings.CountMilliseconds = Time.TotalMilliseconds;
settings.UseSpeechText = UseSpeechText;
settings.SpeechText = SpeechText;
```

```Start``` メソッド全体は次のようになります。
```cs
// 開始ボタンが押された際の処理
private void Start()
{
    // タイマー設定の保存
    var settings = TimerSettings.Instance;
    settings.CountMilliseconds = Time.TotalMilliseconds;
    settings.UseSpeechText = UseSpeechText;
    settings.SpeechText = SpeechText;

    // タイマーを実行してよいかを問い合わせる Alert メッセージの設定
    var alertParameter = new AlertParameter()
    {
        Title = "確認",
        Message = "タイマーを開始します。よろしいですか？",
        Accept = "開始する",
        Cancel = "開始しない",

        // アラートメッセージで「開始する/開始しない」選択後の処理
        Action = result =>
        {
            // 「開始する」の場合、タイマーのカウント画面へ移動するようメッセージを送信
            if (result)
                MessagingCenter.Send(this, "Start");
        }
    };

    // アラートメッセージを表示するようメッセージを送信
    MessagingCenter.Send(this, "DisplayAlert", alertParameter);
}
```

今の段階では ```MainPage.cs``` で ```CountDownPage``` クラスが見つからないというエラーが出ますが、```CountDownPage``` は次ページで作成します。

[< 前ページ](./textbook08.md) | [次ページ >](./textbook10.md)  