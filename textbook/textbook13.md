# 画面スリープしないようにする
スマートデバイスには触らないでいると自動で画面 OFF になる機能があり、一般的に端末の所有者はこの設定を行っています。  
しかし、今回作成しているアプリはタイマーアプリです。この画面が自動で OFF になってはあまり使い勝手の良いものではありません。  

## DependencyService
アプリ起動中に画面が自動で OFF にならないようにするにはプラットフォーム毎の個別の実装が必要です。ここで、前ページの DependencyService が利用できます。  
## 共通コードプロジェクト上の実装
前ページの手順で既に完了しています。

## Android プロジェクト上の実装
Android で画面スリープを OFF にするのは少し厄介です。理由は設定に Activity の参照が必要となっているためです。Android の過去のバージョンでは Activity を取得する static メソッドが用意されていましたが現在は非推奨となっています (Xamarin ではなく Android の SDK です) 。  
今回は起動時に Activity の参照を記録する方針で作成します。

Android プロジェクト (MyTimer.Android または MyTimer.Droid) の ```KeppScreenOn``` クラスに Activity を保存するフィールドとメソッドを作成します。  
**※注意※ iOS プロジェクトにも同名のクラスがあります。間違えないよう注意してください**  

次のコードを ```KeppScreenOn``` クラスに追加してください。
```cs
// Activity を static フィールドに確保
private static Activity _mainActivity;
public static void SetActivity(Activity activity) => _mainActivity = activity;
```

以前の手順で空実装にしておいた ```Set``` メソッドを実装します。  
次のコードで ```KeppScreenOn``` クラスの ```Set``` メソッドを上書きしてください。
```cs
// KeepScreenOn を設定
public void Set(bool keepOn) => _mainActivity.RunOnUiThread(() =>
{
    if (keepOn)
        _mainActivity.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
    else
        _mainActivity.Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
});
```

```KeppScreenOn.cs``` は次のようになります。
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
        // Activity を static フィールドに確保
        private static Activity _mainActivity;
        public static void SetActivity(Activity activity) => _mainActivity = activity;

        // KeepScreenOn を設定
        public void Set(bool keepOn) => _mainActivity.RunOnUiThread(() =>
        {
            if (keepOn)
                _mainActivity.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            else
                _mainActivity.Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
        });
    }
}
```

Android プロジェクトでは MainActivity クラスへもコード追加が必要になります。  
Android プロジェクトの ```MainActivity.cs``` 内の ```MainActivity``` クラスの ```OnCreate``` メソッドに次のコードを追加します。追加する位置は ```global::Xamarin.Forms.Forms.Init(this, bundle);``` の次の行です。
```cs
KeppScreenOn.SetActivity(this);
```

```MainActivity.cs``` は次のようになります。
```cs
using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MyTimer.Droid
{
    [Activity(Label = "MyTimer", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            KeppScreenOn.SetActivity(this);
            LoadApplication(new App());
        }
    }
}
```

## iOS プロジェクト上の実装
iOS は Android と比べて手順が少なくて済みます。

以前の手順で空実装にしておいた ```Set``` メソッドを実装します。  
次のコードで ```KeppScreenOn``` クラスの ```Set``` メソッドを上書きしてください。  
**※注意※ Android プロジェクトにも同名のクラスがあります。間違えないよう注意してください**  
```cs
// KeepScreenOn を設定
public void Set(bool keepOn) => UIApplication.SharedApplication.InvokeOnMainThread(() =>
                UIApplication.SharedApplication.IdleTimerDisabled = keepOn);
```

```KeppScreenOn.cs``` は次のようになります。
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
        public void Set(bool keepOn) => UIApplication.SharedApplication.InvokeOnMainThread(() =>
                        UIApplication.SharedApplication.IdleTimerDisabled = keepOn);
    }
}
```

[< 前ページ](./textbook12.md) | [次ページ >](./textbook14.md)  