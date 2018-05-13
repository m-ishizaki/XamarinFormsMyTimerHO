# データバインディング
View と ViewModel とでデータを同期するデータバインディングを設定します。  
  
## ViewModel の実装
ViewModel の実装は前ページの手順で完了しています。  

## View の実装
ViewModel のプロパティと View 要素をデータバインディングによりバインディングします。  
データバインディングにより、View の値と ViewModel の値が同期し、一方が変わるともう一方も変わるようになります。  
また、ボタンタップ時にはバインディングされた ViewModel のコマンドプロパティの処理が実行されます。  

## BindingContext ぼ設定
View にバインディングされる ViewModel を指定します。  
```MainPage.xaml``` の ```ContentPage``` の子要素として、次のコードを追加してください。  
```xml
<!-- ViewModel を設定、構築 -->
<ContentPage.BindingContext>
    <vm:MainPageViewModel />
</ContentPage.BindingContext>
```
貼り付け後は次のようになります。 (抜粋)
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyTimer"
             x:Class="MyTimer.MainPage"
             xmlns:vm="clr-namespace:MyTimer"
             Title="オリジナルタイマーアプリ">
    <!-- ViewModel を設定、構築 -->
    <ContentPage.BindingContext>
        <vm:MainPageViewModel />
    </ContentPage.BindingContext>

    <!-- ページの表示内容 -->
    <ContentPage.Content>
```
XAML はオブジェクトを構築する設計書です。  
ここでは、```ContentPage``` オブジェクトの ```BindingContext``` プロパティに ```MainPageViewModel‘‘‘ クラスをインスタンス化し設定する、という指定をしています。

## 値のバインディング
View の値と ViewModel のプロパティをバインディングします。  
**「設定中のタイマー時間表示」** とコメントのついたラベルを次のように書き換えます。
```xml
<!-- 設定中のタイマー時間表示 -->
<Label Text="{Binding Time, StringFormat='{0:mm\\:ss}'}"
    Grid.ColumnSpan="2"
    HorizontalOptions="Center" VerticalOptions="Center"
    FontSize="Large" TextColor="Black" FontAttributes="Bold"/>
```
この ```{Binding``` の後に書かれた名前の ViewModel のプロパティがこのラベルのテキストと同期されるようになります。  
また、```StringFormat``` を指定することにより表示のフォーマットを指定できます。ここでは **「00:00」（分:秒）** というフォーマットで表示される指定をしています。  
  
同様に他の値もバインディングします。  

**「テキスト読み上げ or 音声ファイル の選択 Switch」** とコメントのついたスイッチを次のように書き換えます。
```xml
<Switch x:Name="useSpeechText" IsToggled="{Binding UseSpeechText}" />
```

**「テキスト読み上げのテキスト」** とコメントのついたエディターを次のように書き換えます。
```xml
<Editor Text="{Binding SpeechText}"
    HorizontalOptions="Fill"
    IsVisible="{Binding IsToggled, Source={x:Reference useSpeechText}" BackgroundColor="White"/>
```
```IsVisible``` にも ```{Binding``` とありますが、このように書くことで View の要素同士で値のバインディングが可能です。  
ここでは、スイッチの選択に応じてエディターが表示/非表示が切り替わります。  

## コマンドのバインディング
コマンドをボタンの Command プロパティにバインディングします。  

**「タイマー開始ボタン」** とコメントのついたボタンを次のように書き換えます。
```xml
<!-- タイマー開始ボタン -->
<Button Text="タイマー開始" Command="{Binding StartCommand}" 
    Grid.Row="1" Grid.RowSpan="2"
    BackgroundColor="#80808080"/>
```

## パラメーター付きのコマンドのバインディング
タイマー設定時間を増減するボタンにコマンドをバインディングします。  
コマンドのバインディングは値のバインディングと異なり、```CommandParameter``` を指定できます。```CommandParameter``` で指定された値はそのコマンドが実行されたときにメソッドの引数として渡されます。  
今回はタイマー設定の増減ボタンそれぞれの ```CommandParameter``` で、「1、-1、10、-10」に設定することで、一つのコマンドで  

- 10 の位の増
- 10 の位の減
- 1 の位の増
- 1 の位の減

を実現しています。  

**「「分」 ラベル」** というコメントの 4 行下の **「10の位設定」** とコメントのついた StackLayout を次のように書き換えます。
```xml
<!-- 10の位設定 -->
<StackLayout>
    <Label Text="10" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
    <Button Text="＋" Command="{Binding AddMinutesCommand}" CommandParameter="10" BackgroundColor="#80808080"/>
    <Button Text="ー" Command="{Binding AddMinutesCommand}" CommandParameter="-10" BackgroundColor="#80808080"/>
</StackLayout>
```

同様にすぐ下の **「1の位設定」** とコメントのついた StackLayout を次のように書き換えます。
```xml
<!-- 1の位設定 -->
<StackLayout>
    <Label Text="1" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
    <Button Text="＋" Command="{Binding AddMinutesCommand}" CommandParameter="1" BackgroundColor="#80808080"/>
    <Button Text="ー" Command="{Binding AddMinutesCommand}" CommandParameter="-1" BackgroundColor="#80808080"/>
</StackLayout>
```

同様に、秒単位の設定ボタンにもバインディングします。  
**<!-- 秒の + - ボタン達 -->** コメントのついた StackLayout を次のように書き換えます。
```xml
<!-- 秒の + - ボタン達 -->
<StackLayout>
    <!-- 「秒」 ラベル -->
    <Label Text="秒" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
    <!-- + - ボタン達  -->
    <StackLayout Orientation="Horizontal">
        <!-- 10の位設定 -->
        <StackLayout>
            <Label Text="10" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
            <Button Text="＋" Command="{Binding AddSecondsCommand}" CommandParameter="10" BackgroundColor="#80808080"/>
            <Button Text="ー" Command="{Binding AddSecondsCommand}" CommandParameter="-10" BackgroundColor="#80808080"/>
        </StackLayout>
        <!-- 1の位設定 -->
        <StackLayout>
            <Label Text="1" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
            <Button Text="＋" Command="{Binding AddSecondsCommand}" CommandParameter="1" BackgroundColor="#80808080"/>
            <Button Text="ー" Command="{Binding AddSecondsCommand}" CommandParameter="-1" BackgroundColor="#80808080"/>
        </StackLayout>
    </StackLayout>
</StackLayout>
```


ここまでの手順を終えた ```MainPageViewModel.cs``` は次のようになっています。
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyTimer"
             x:Class="MyTimer.MainPage"
             xmlns:vm="clr-namespace:MyTimer"
             Title="オリジナルタイマーアプリ">
    <!-- ViewModel を設定、構築 -->
    <ContentPage.BindingContext>
        <vm:MainPageViewModel />
    </ContentPage.BindingContext>

    <!-- ページの表示内容 -->
    <ContentPage.Content>
        <Grid>
            <!-- 背景画像 -->
            <Image Source="{local:ImageResource MyTimer.Resources.Background.png}" Aspect="AspectFit" />

            <Grid BackgroundColor="#80FFFFFF">
                <Grid.RowDefinitions>
                    <RowDefinition Height="*"/>
                    <RowDefinition Height="Auto"/>
                    <RowDefinition Height="40"/>
                </Grid.RowDefinitions>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>

                <!-- 設定中のタイマー時間表示 -->
                <Label Text="{Binding Time, StringFormat='{0:mm\\:ss}'}"
                    Grid.ColumnSpan="2"
                    HorizontalOptions="Center" VerticalOptions="Center"
                    FontSize="Large" TextColor="Black" FontAttributes="Bold"/>

                <!-- タイマー開始ボタン -->
                <Button Text="タイマー開始" Command="{Binding StartCommand}" 
                    Grid.Row="1" Grid.RowSpan="2"
                    BackgroundColor="#80808080"/>

                <!-- タイマー時間の + - ボタン達 -->
                <StackLayout Orientation="Horizontal"
                    Grid.Row="1" Grid.Column="1"
                    HorizontalOptions="Center">
                    <!-- 分の + - ボタン達 -->
                    <StackLayout>
                        <!-- 「分」 ラベル -->
                        <Label Text="分" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
                        <!-- + - ボタン達  -->
                        <StackLayout Orientation="Horizontal">
                            <!-- 10の位設定 -->
                            <StackLayout>
                                <Label Text="10" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
                                <Button Text="＋" Command="{Binding AddMinutesCommand}" CommandParameter="10" BackgroundColor="#80808080"/>
                                <Button Text="ー" Command="{Binding AddMinutesCommand}" CommandParameter="-10" BackgroundColor="#80808080"/>
                            </StackLayout>
                            <!-- 1の位設定 -->
                            <StackLayout>
                                <Label Text="1" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
                                <Button Text="＋" Command="{Binding AddMinutesCommand}" CommandParameter="1" BackgroundColor="#80808080"/>
                                <Button Text="ー" Command="{Binding AddMinutesCommand}" CommandParameter="-1" BackgroundColor="#80808080"/>
                            </StackLayout>
                        </StackLayout>
                    </StackLayout>
                    <!-- 秒の + - ボタン達 -->
                    <StackLayout>
                        <!-- 「秒」 ラベル -->
                        <Label Text="秒" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
                        <!-- + - ボタン達  -->
                        <StackLayout Orientation="Horizontal">
                            <!-- 10の位設定 -->
                            <StackLayout>
                                <Label Text="10" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
                                <Button Text="＋" Command="{Binding AddSecondsCommand}" CommandParameter="10" BackgroundColor="#80808080"/>
                                <Button Text="ー" Command="{Binding AddSecondsCommand}" CommandParameter="-10" BackgroundColor="#80808080"/>
                            </StackLayout>
                            <!-- 1の位設定 -->
                            <StackLayout>
                                <Label Text="1" HorizontalOptions="Center" TextColor="Black" FontAttributes="Bold"/>
                                <Button Text="＋" Command="{Binding AddSecondsCommand}" CommandParameter="1" BackgroundColor="#80808080"/>
                                <Button Text="ー" Command="{Binding AddSecondsCommand}" CommandParameter="-1" BackgroundColor="#80808080"/>
                            </StackLayout>
                        </StackLayout>
                    </StackLayout>
                </StackLayout>

                <!-- カウント後の音声設定 -->
                <Grid Grid.Row="2" Grid.Column="1">
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="Auto"/>
                        <ColumnDefinition Width="*"/>
                    </Grid.ColumnDefinitions>
                    <!-- テキスト読み上げ or 音声ファイル の選択 Switch -->
                    <Switch x:Name="useSpeechText" IsToggled="{Binding UseSpeechText}" />
                    <Grid Grid.Column="1">
                        <!-- 音声ファイルを使用ラベル -->
                        <Label Text="音声ファイルを使用"
                            HorizontalOptions="Fill" VerticalOptions="Center" HorizontalTextAlignment="Start"
                            TextColor="Black" FontAttributes="Bold"/>
                        <!-- テキスト読み上げのテキスト -->
                        <Editor Text="{Binding SpeechText}"
                            HorizontalOptions="Fill"
                            IsVisible="{Binding IsToggled, Source={x:Reference useSpeechText}" BackgroundColor="White"/>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    </ContentPage.Content>
</ContentPage>
```

## デバッグ実行
デバッグ実行をします。  
「**＋**」 や 「**ー**」 ボタンで画面上部中央のタイマー設定が変更されればこのページは完了です。 

[< 前ページ](./textbook06.md) | [次ページ >](./textbook08.md)  