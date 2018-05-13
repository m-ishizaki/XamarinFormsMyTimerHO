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
