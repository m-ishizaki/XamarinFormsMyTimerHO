using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyTimer
{
    // 起動直後の画面。タイマー設定を行う画面
    public partial class MainPage : ContentPage
	{
        // コンストラクタ
        public MainPage()
		{
			InitializeComponent();
		}

        // 画面が表示されたタイミングでの処理
        private void MainPageAppearing(object sender, EventArgs e)
        {
            // メッセージの購読を設定する
            // アラートダイアログ表示メッセージを購読する
            MessagingCenter.Subscribe<MainPageViewModel, AlertParameter>(this, "DisplayAlert", DisplayAlert);
            // タイマースタートのメッセージを購読する
            MessagingCenter.Subscribe<MainPageViewModel>(this, "Start", StartTimer);
        }

        // 画面が表示されなくなったタイミングでの処理
        private void MainPageDisappearing(object sender, EventArgs e)
        {
            // メッセージの購読を解除する
            MessagingCenter.Unsubscribe<MainPageViewModel, AlertParameter>(this, "DisplayAlert");
            MessagingCenter.Unsubscribe<MainPageViewModel>(this, "Start");
        }

        // アラートダイアログを表示する
        private async void DisplayAlert<T>(T sender, AlertParameter arg)
        {
            // アラートダイアログを表示する
            var isAccept = await DisplayAlert(arg.Title, arg.Message, arg.Accept, arg.Cancel);
            // アラートダイアログでのユーザーの選択結果い応じた処理を実行する
            arg.Action?.Invoke(isAccept);
        }

        // タイマーをスタートする
        private void StartTimer<T>(T sender)
        {
            // タイマー画面へ遷移する
            this.Navigation.PushModalAsync(new CountDownPage());
        }

    }
}
