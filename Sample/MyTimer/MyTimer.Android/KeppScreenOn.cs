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