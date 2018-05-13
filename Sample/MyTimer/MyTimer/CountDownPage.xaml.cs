using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyTimer
{
    // タイマーカウントダウン画面
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountDownPage : ContentPage
    {
        // コンストラクタ
        public CountDownPage()
        {
            InitializeComponent();
        }

        // 画面が表示されたタイミングでの処理
        private void CountDownPageAppearing(object sender, EventArgs e)
        {
            // 戻るメッセージを購読する
            MessagingCenter.Subscribe<CountDownPageViewModel>(this, "GoBack", GoBack);
        }

        // 画面が表示されなくなったタイミングでの処理
        private void CountDownPageDisappearing(object sender, EventArgs e)
        {
            // メッセージの購読を解除する
            MessagingCenter.Unsubscribe<CountDownPageViewModel>(this, "GoBack");
        }

        // 画面を閉じ、タイマー設定画面へ戻る
        private void GoBack<T>(T sender)
        {
            // タイマー設定画面へ遷移する
            this.Navigation.PopModalAsync();
        }
    }
}