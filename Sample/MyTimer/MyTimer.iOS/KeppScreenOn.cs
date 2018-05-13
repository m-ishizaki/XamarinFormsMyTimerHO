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