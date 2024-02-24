using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace AnalogClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 時刻表示用タイマー
        /// </summary>
        private DispatcherTimer timer;

        public MainWindow()
        {
            Topmost = Properties.Settings.Default.TopMost;
            InitializeComponent();
            InitializeAngle(-45);

            //コンテンツに合わせて自動的にサイズを設定
            SizeToContent = SizeToContent.WidthAndHeight;

            MouseLeftButtonDown += (o, e) => DragMove();

            // タイマー生成
            timer = CreateTimer();

            timer.Start();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartAnimation("HourHand", AngleHour.Angle);
            StartAnimation("MinuteHand", AngleMinute.Angle);
            StartAnimation("SecondHand", AngleSecond.Angle);
        }

        void InitializeAngle(int init = 0)
        {
            DateTime dt = DateTime.Now;
            AngleSecond.Angle = dt.Second * 360.0 / 60.0 + init;
            AngleMinute.Angle = (dt.Minute + dt.Second / 60.0) * 360.0 / 60.0 + init;
            AngleHour.Angle = (dt.Hour + dt.Minute / 60.0) * 360.0 / 12 + init;
        }

        private void StartAnimation(string name, double angle)
        {
            var sb = Resources[name] as Storyboard;
            var da = sb.Children[0] as DoubleAnimation;
            da.From = angle;
            da.To = da.From + 360.0;
            sb.Begin();
        }

        //チェック時
        private void fixedFront_Checked(object s, RoutedEventArgs e)
        {
            Topmost = true;
            Properties.Settings.Default.TopMost = Topmost;
            Properties.Settings.Default.Save();
        }

        //未チェック時
        private void fixedFront_Unchecked(object s, RoutedEventArgs e)
        {
            Topmost = false;
            Properties.Settings.Default.TopMost = Topmost;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// タイマー生成処理
        /// </summary>
        /// <returns>生成したタイマー</returns>
        private DispatcherTimer CreateTimer()
        {
            // タイマー生成（優先度はアイドル時に設定）
            var t = new DispatcherTimer(DispatcherPriority.SystemIdle);

            // タイマーイベントの発生間隔を300ミリ秒に設定
            t.Interval = TimeSpan.FromMilliseconds(300);



            // タイマーイベントの定義
            t.Tick += (sender, e) =>
            {
                // 現在の時分秒をテキストに設定
                textBlock.Text = DateTime.Now.ToString("HH:mm");
            };

            // 生成したタイマーを返す
            return t;
        }
    }
}
