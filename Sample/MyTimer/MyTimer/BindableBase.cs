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
