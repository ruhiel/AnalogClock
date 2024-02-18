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

namespace AnalogClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeAngle(-45);

            //コンテンツに合わせて自動的にサイズを設定
            this.SizeToContent = SizeToContent.WidthAndHeight;

            MouseLeftButtonDown += (o, e) => DragMove();
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
    }
}
