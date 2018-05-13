# ViewModelの作成
画面のデータと機能を持つ ViewModel クラスを作成します。  
本来は機能は Model クラスに持つことが望ましいですが、今回は手順を減らすために ViewModel クラスに実装します。  

## クラスの新規作成
```MyTimer``` プロジェクト( 共有コードプロジェクト) に次の二つのクラスを作成します。  
クラスの作成方法は [背景画像の表示](./textbook03.md) で学んでいます。思い出しながら作成してください。  
中身は初期状態のままで OK です。

- ```MainPageViewModel``` クラス
- ```BindableBase``` クラス

## BindableBase の実装
次ページで解説するデータバインディングを実装するためのクラスです。  
このクラスは定型処理として毎回コピー＆ペーストで再利用して構いません。  
中身については理解に比較的高度な C# 力が要求されまので、今回はコピー＆ペーストで済ませてしまいましょう。  
中身は時間のある時にゆっくりと確認してください。```INotifyPropertyChanged``` インタフェースがポイントなので、これをキーワードに調べると良いでしょう。     

今回は次のコードで ```BindableBase.cs``` ファイルの内容を上書きしてください。
```cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyTimer
{
    // プロパティの変更通知機能を持ったモデルクラスのベースクラス
    // 実際としては ViewModel クラスのベースクラス
    class BindableBase : INotifyPropertyChanged
    {
        // プロパティ変更通知イベント
        public event PropertyChangedEventHandler PropertyChanged;

        //プロパティ変更通知イベントを発生させる
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // プロパティの値をセットする
        // プロパティの値が変更されない場合はセットをしない
        // プロパティの値が変更される場合は値を更新し、変更通知のイベントを発生させる
        protected bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(property, value)) return false;
            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
```

## MainPageViewModel の実装
メインページのデータと機能を持つクラスです。  
ViewModel クラスは、次ページで解説するデータバインディングを実装するために ```BindableBase``` クラスを基底クラス (ベースクラスやスーパークラスなどとも呼ばれる) として継承した派生クラス (導出クラスやサブクラスなどとも呼ばれる) として実装します。  

C# では基底クラスの指定は、クラス名 に続けて ```:``` + 基底クラス名 を記述します。  
今回は次のコードで ```MainPageViewModel.cs``` ファイルの内容を上書きしてください。
```cs
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyTimer
{
    // 起動直後のタイマー設定画面の ViewModel
    class MainPageViewModel : BindableBase
    {

    }
}
```

## データを保持するプロパティの実装
```MainPageViewModel``` クラスにページのデータをもつプロパティを定義します。  
プロパティとは、クラスの外から値の設定または取得のできるメンバーで、今回は View から値の設定と取得が行われます。  
次のコードで出てくる ```SetProperty``` メソッドは ```BindableBase``` クラスに実装したメソッドで、値が変更されたことを View に通知してくれるすごいメソッドです。  
  
次のコードを ```MainPageViewModel``` クラスに追加してください。  
```cs
// **画面の値とバインディングするプロパティ**

// タイマー時間
private TimeSpan _time = TimeSpan.FromMinutes(5);
public TimeSpan Time { get { return _time; }set { SetProperty(ref _time, value); } }

// タイマー時間経過後の案内音声
private string _speechText = "Time haspassed";
public string SpeechText { get { return _speechText; } set { SetProperty(ref _speechText, value); } }

// タイマー時間経過後に案内音声を使うか？（使わない場合、Audio 再生）
private bool _useSpeechText = true;
public bool UseSpeechText { get { return _useSpeechText; } set { SetProperty(ref _useSpeechText, value); } }
```

ここまでの手順を終えた ```MainPageViewModel.cs``` は次のようになっています。
```cs
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyTimer
{
    // 起動直後のタイマー設定画面の ViewModel
    class MainPageViewModel : BindableBase
    {
        // **画面の値とバインディングするプロパティ**

        // タイマー時間
        private TimeSpan _time = TimeSpan.FromMinutes(5);
        public TimeSpan Time { get { return _time; } set { SetProperty(ref _time, value); } }

        // タイマー時間経過後の案内音声
        private string _speechText = "Time haspassed";
        public string SpeechText { get { return _speechText; } set { SetProperty(ref _speechText, value); } }

        // タイマー時間経過後に案内音声を使うか？（使わない場合、Audio 再生）
        private bool _useSpeechText = true;
        public bool UseSpeechText { get { return _useSpeechText; } set { SetProperty(ref _useSpeechText, value); } }
    }
}
```

## コマンドプロパティの実装
```MainPageViewModel``` クラスにページの機能の呼び出し口となるコマンドを定義します。  
コマンドとは、プロパティと同じようにクラスの外から参照できるメンバーで、 View からの取得だけが行われます。多くは View のボタンなどに紐づけられ、ボタンが押された際にコマンドを通じて ViewModel の機能が呼び出されます。  
  
次のコードを ```MainPageViewModel``` クラスに追加してください。  
```cs
// **画面のボタンとバインディングするコマンド**

// 開始ボタンが押された
public Command StartCommand { get; }

// 秒 +/- ボタンが押された
public Command AddSecondsCommand { get; }

// 分 +/- ボタンが押された
public Command AddMinutesCommand { get; }
```

ここまでの手順を終えた現在の ```MainPageViewModel.cs``` は次のようになっています。
```cs
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyTimer
{
    // 起動直後のタイマー設定画面の ViewModel
    class MainPageViewModel : BindableBase
    {
        // **画面の値とバインディングするプロパティ**

        // タイマー時間
        private TimeSpan _time = TimeSpan.FromMinutes(5);
        public TimeSpan Time { get { return _time; } set { SetProperty(ref _time, value); } }

        // タイマー時間経過後の案内音声
        private string _speechText = "Time haspassed";
        public string SpeechText { get { return _speechText; } set { SetProperty(ref _speechText, value); } }

        // タイマー時間経過後に案内音声を使うか？（使わない場合、Audio 再生）
        private bool _useSpeechText = true;
        public bool UseSpeechText { get { return _useSpeechText; } set { SetProperty(ref _useSpeechText, value); } }


        // **画面のボタンとバインディングするコマンド**

        // 開始ボタンが押された
        public Command StartCommand { get; }

        // 秒 +/- ボタンが押された
        public Command AddSecondsCommand { get; }

        // 分 +/- ボタンが押された
        public Command AddMinutesCommand { get; }
    }
}
```

## 機能の実装
```MainPageViewModel``` クラスにページの機能を実装します。  

機能はコマンドを通じて呼び出されるため外部から参照できる必要はありません。そのため private メソッドとして実装します。  
次のコードを ```MainPageViewModel``` クラスに追加してください。  

**・ParseLong メソッド**  
文字列を数値に変換するメソッドです。  
```cs
// **複数個所で行われる処理をメソッド化したメソッド**

// object (中身は string) を long に parse する
private long ParseLong(object arg)
{
    if (long.TryParse(arg?.ToString(), out var value))
        return value;
    return default(long);
}
```

**・AddTime メソッド**  
タイマー設定時間を増減するメソッドで、```Time``` プロパティの値を変更します。マイナス値を設定することで値の減少ができます。
内部で、```ParseLong``` メソッドを使用します。
```cs
// タイマー時間を追加する
private void AddTime(long seconds)
{
    var newTime = Time.TotalSeconds + seconds;

    // 必ず 1 秒以上 60 分未満となるよう調整する
    newTime = Math.Max(1, newTime);
    newTime = Math.Min((60 * 60) - 1, newTime);

    Time = TimeSpan.FromSeconds(newTime);
}
```

**・AddSeconds メソッド**  
タイマー設定時間の秒を増減するメソッドです。マイナス値を設定することで値の減少ができます。  
内部で、```AddTime``` メソッドを使用します。
```cs
// **コマンドの中身となるボタンが押された際の処理メソッド**

// 秒 +/- ボタンが押された際の処理
private void AddSeconds(object parameter)
{
    long value = ParseLong(parameter);
    AddTime(value);
}
```

**・AddMinutes メソッド**  
タイマー設定時間の分を増減するメソッドです。マイナス値を設定することで値の減少ができます。  
内部で、```AddTime``` メソッドを使用します。
```cs
// 分 +/- ボタンが押された際の処理
private void AddMinutes(object parameter)
{
    long value = ParseLong(parameter);
    AddTime(value * 60);
}
```

**・```Start``` メソッド**  
タイマーのカウント画面へ遷移し、タイマーを開始します。実装は後の手順で行います。
```cs
// 開始ボタンが押された際の処理
private void Start()
{
}
```

## コンストラクタの実装 (コマンドのインスタンス化)
```MainPageViewModel``` クラスのコンストラクタでコマンドのインスタンス化を行います。  
C# ではコンストラクタ内で読み取り専用プロパティの値の設定が行えます (コマンドは読み取り専用プロパティとして宣言しています) 。  
※読み取り専用なので、通常のメソッド内では値の設定は行えません。  

次のコードを ```MainPageViewModel``` クラスに追加してください。  
```cs
// **コンストラクタ**

// コンストラクタ
public MainPageViewModel()
{
    // コマンドの設定
    // readonly プロパティの初期化は、コンストラクタ内でも行える
    AddSecondsCommand = new Command(AddSeconds);
    AddMinutesCommand = new Command(AddMinutes);
    StartCommand = new Command(Start);
}
```

ここまでの手順を終えた ```MainPageViewModel.cs``` は次のようになっています。
```cs
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyTimer
{
    // 起動直後のタイマー設定画面の ViewModel
    class MainPageViewModel : BindableBase
    {
        // **画面の値とバインディングするプロパティ**

        // タイマー時間
        private TimeSpan _time = TimeSpan.FromMinutes(5);
        public TimeSpan Time { get { return _time; } set { SetProperty(ref _time, value); } }

        // タイマー時間経過後の案内音声
        private string _speechText = "Time haspassed";
        public string SpeechText { get { return _speechText; } set { SetProperty(ref _speechText, value); } }

        // タイマー時間経過後に案内音声を使うか？（使わない場合、Audio 再生）
        private bool _useSpeechText = true;
        public bool UseSpeechText { get { return _useSpeechText; } set { SetProperty(ref _useSpeechText, value); } }


        // **画面のボタンとバインディングするコマンド**

        // 開始ボタンが押された
        public Command StartCommand { get; }

        // 秒 +/- ボタンが押された
        public Command AddSecondsCommand { get; }

        // 分 +/- ボタンが押された
        public Command AddMinutesCommand { get; }


        // **複数個所で行われる処理をメソッド化したメソッド**

        // object (中身は string) を long に parse する
        private long ParseLong(object arg)
        {
            if (long.TryParse(arg?.ToString(), out var value))
                return value;
            return default(long);
        }

        // タイマー時間を追加する
        private void AddTime(long seconds)
        {
            var newTime = Time.TotalSeconds + seconds;

            // 必ず 1 秒以上 60 分未満となるよう調整する
            newTime = Math.Max(1, newTime);
            newTime = Math.Min((60 * 60) - 1, newTime);

            Time = TimeSpan.FromSeconds(newTime);
        }


        // **コマンドの中身となるボタンが押された際の処理メソッド**

        // 秒 +/- ボタンが押された際の処理
        private void AddSeconds(object parameter)
        {
            long value = ParseLong(parameter);
            AddTime(value);
        }

        // 分 +/- ボタンが押された際の処理
        private void AddMinutes(object parameter)
        {
            long value = ParseLong(parameter);
            AddTime(value * 60);
        }

        // 開始ボタンが押された際の処理
        private void Start()
        {
        }


        // **コンストラクタ**

        // コンストラクタ
        public MainPageViewModel()
        {
            // コマンドの設定
            // readonly プロパティの初期化は、コンストラクタ内でも行える
            AddSecondsCommand = new Command(AddSeconds);
            AddMinutesCommand = new Command(AddMinutes);
            StartCommand = new Command(Start);
        }
    }
}
```

[< 前ページ](./textbook05.md) | [次ページ >](./textbook07.md)  