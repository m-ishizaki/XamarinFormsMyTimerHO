# DependencyService
タイマーアプリの機能としてタイマー設定時間が経過したことを音で伝えます。  
音声ファイルの再生などのプラットフォームごとの機能は、Xamarin.Forms では機能が用意されていません (Xamarin.Android や Xamarin.iOS では対応しています) 。  
このような機能は DependencyService を使うことで簡単に実装できます。  

## DependencyService 概要
プロジェクト構成上、プラットフォーム機能は MyTimer.Android や MyTimer.iOS といったプラットフォーム毎のプロジェクトに作成します。そしてそれら作成した処理は、MyTimer プロジェクトといった共有コードのプロジェクトから呼び出して起動します。  
しかしプロジェクト間の依存の方向として、共有コードプロジェクトからプラットフォームのプロジェクトを参照することはできません。この制約を越えるための手段の一つが DependencyService です。  
  
## 共有コードプロジェクト上の実装
共有コードプロジェクト (MyTimer プロジェクト) にインターフェイスを作成します。今回は DependencyService を使って画面の自動スリープを止める機能を作成するので、それを前提とした名前で作成して行きます。  

インターフェイスの作成方法はクラスや View の作成と基本的に同じ流れになります。思い出しながら作成してください。  
  
**Mac の場合**  
・ソリューションエクスプローラー上の [MyTimer] 上で二本指タップ (右クリック) します。  
・[追加 > 新しいファイル] を選択します。  
・[新しいファイル] ウィンドウで [General > 空のインターフェイス] を選択します。  
・[名前:] に ```IKeepScreenOn``` と入力します。  
・[新規] をクリックします。  

**Windows の場合**  
・ソリューションエクスプローラー上の [MyTimer] 上で二本指タップ(右クリック)します。  
・[追加 > 新しい項目] を選択します。  
・[新しい項目の追加 - MyTimer] ウィンドウの左のペインで [インストール済み > Visual C# アイテム > コード] を選択します。  
・右のペインで [インターフェイス] を選択します。  
・[名前:] に ```IKeepScreenOn``` と入力します。  
・[追加] をクリックします。

**共通**  
機能を呼び出すメソッドを定義します。今回は画面スリープを止める機能の ON/OFF を設定する ```Set``` メソッドを定義します。  
次のコードで ```IKeepScreenOn.cs``` を上書きしてください。
```cs
using System;
using System.Collections.Generic;
using System.Text;

namespace MyTimer
{
    // / KeepScreenOn の DependencyService 用のインターフェイス
    public interface IKeepScreenOn
    {
        // KeepScreenOn を設定
        void Set(bool keepOn);
    }
}
```

## Android プロジェクト上の実装
Android プロジェクト (MyTimer.Android または MyTimer.Droid) に実装クラスを作成します。  
**※注意※ これまでの MyTimer プロジェクトではない点に注意してください**  
クラスの作成方法は [背景画像の表示](./textbook03.md) で学んでいます。思い出しながら作成してください。  
**※最初の二本指タップ(右クリック)が、ソリューションエクスプローラー上の [MyTimer.Android] または [MyTimer.Droid] になる点に注意してください**  

- ```KeppScreenOn``` クラス

作成した ```KeppScreenOn``` で ```IKeepScreenOn``` を実装します。  
次のコードを ```KeppScreenOn.cs``` ファイルのクラス名の宣言を次のように書き換えます。
```cs
// KeepScreenOn の Android の実装
class KeppScreenOn : IKeepScreenOn
```
インターフェイスで定義されたメソッドも実装します。ここではまだ機能は作成しないので空の実装です。  
次のコードを ```KeppScreenOn``` クラスに追加します。
```cs
// KeepScreenOn を設定
public void Set(bool keepOn){ ; }
```

DependencyService の実装クラスは assembly   
 ```KeppScreenOn.cs``` ファイルの namespace 指定の前に追加します。
```cs
// Dependency Service に Android の実装を登録
[assembly: Xamarin.Forms.Dependency(typeof(MyTimer.Droid.KeppScreenOn))]
```

ここまでの ```KeppScreenOn.cs``` のコードは次のようになります。
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

// Dependency Service に Android の実装を登録
[assembly: Xamarin.Forms.Dependency(typeof(MyTimer.Droid.KeppScreenOn))]
namespace MyTimer.Droid
{
    // KeepScreenOn の Android の実装
    class KeppScreenOn : IKeepScreenOn
    {
        // KeepScreenOn を設定
        public void Set(bool keepOn){ ; }
    }
}
```

## iOS プロジェクト上の実装
iOS プロジェクト (MyTimer.iOS) に実装クラスを作成します。  
**※注意※ これまでの MyTimer プロジェクトではない点に注意してください**  
クラスの作成方法は [背景画像の表示](./textbook03.md) で学んでいます。思い出しながら作成してください。  
**※最初の二本指タップ(右クリック)が、ソリューションエクスプローラー上の [MyTimer.iOS] になる点に注意してください**  

- ```KeppScreenOn``` クラス

作成した ```KeppScreenOn``` で ```IKeepScreenOn``` を実装します。次のコードを ```KeppScreenOn.cs``` ファイルのクラス名の宣言を次のように書き換えます。

```cs
// KeepScreenOn の iOS の実装
class KeppScreenOn : IKeepScreenOn
```
インターフェイスで定義されたメソッドも実装します。ここではまだ機能は作成しないので空の実装です。  
次のコードを ```KeppScreenOn``` クラスに追加します。
```cs
// KeepScreenOn を設定
public void Set(bool keepOn){ ; }
```

DependencyService の実装クラスは assembly 属性の設定も必要です。  
次のコードを ```KeppScreenOn.cs``` ファイルの namespace 指定の前に追加します。
```cs
// Dependency Service に iOS の実装を登録
[assembly: Xamarin.Forms.Dependency(typeof(MyTimer.iOS.KeppScreenOn))]
```

ここまで手順で ```KeppScreenOn.cs``` は次のようになります。
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

// Dependency Service に iOS の実装を登録
[assembly: Xamarin.Forms.Dependency(typeof(MyTimer.iOS.KeppScreenOn))]
namespace MyTimer.iOS
{
    // KeepScreenOn の iOS の実装
    class KeppScreenOn : IKeepScreenOn
    {
        // KeepScreenOn を設定
        public void Set(bool keepOn){ ; }
    }
}
```

## 機能の実行
共有コードプロジェクト (MyTimer プロジェクト) からプラットフォームのプロジェクトの ```KeppScreenOn``` クラスを呼び出します。  
```Xamarin.Forms.DependencyService``` クラスの ```Get<T>()``` メソッドで先ほど作成したクラスを呼び出せます。

共有コードプロジェクト (MyTimer プロジェクト) の ```CountDownPageViewModel``` クラスのコンストラクターに次のコードを追加します。
```cs
// 画面をスリープしない設定
DependencyService.Get<IKeepScreenOn>().Set(true);
```

```CountDownPageViewModel``` クラスのコンストラクターは次のようになります。
```cs
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
```

[< 前ページ](./textbook11.md) | [次ページ >](./textbook13.md)  