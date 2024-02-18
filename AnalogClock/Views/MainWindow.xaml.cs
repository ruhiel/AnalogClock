using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

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
            InitializeComponent();
            InitializeAngle(-45);

            //コンテンツに合わせて自動的にサイズを設定
            this.SizeToContent = SizeToContent.WidthAndHeight;

            MouseLeftButtonDown += (o, e) => DragMove();

            // タイマー生成
            timer = CreateTimer();

            timer.Start();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartAnimation("HourHand", this.AngleHour.Angle);
            StartAnimation("MinuteHand", this.AngleMinute.Angle);
            StartAnimation("SecondHand", this.AngleSecond.Angle);
        }

        void InitializeAngle(int init = 0)
        {
            DateTime dt = DateTime.Now;
            this.AngleSecond.Angle = dt.Second * 360.0 / 60.0 + init;
            this.AngleMinute.Angle = (dt.Minute + dt.Second / 60.0) * 360.0 / 60.0 + init;
            this.AngleHour.Angle = (dt.Hour + dt.Minute / 60.0) * 360.0 / 12 + init;
        }

        private void StartAnimation(string name, double angle)
        {
            var sb = this.Resources[name] as Storyboard;
            var da = sb.Children[0] as DoubleAnimation;
            da.From = angle;
            da.To = da.From + 360.0;
            sb.Begin();
        }

        //チェック時
        private void fixedFront_Checked(object s, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        //未チェック時
        private void fixedFront_Unchecked(object s, RoutedEventArgs e)
        {
            this.Topmost = false;
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
