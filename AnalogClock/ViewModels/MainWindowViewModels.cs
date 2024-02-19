using AnalogClock.Views;
using Reactive.Bindings;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnalogClock.ViewModels
{
    public class MainWindowViewModels
    {
        public ReactiveCommand<EventArgs> SettingCommand { get; } = new ReactiveCommand<EventArgs>();
        public ReactiveCommand<EventArgs> ExitCommand { get; } = new ReactiveCommand<EventArgs>();
        public ReactiveProperty<ImageSource> Source { get; } = new ReactiveProperty<ImageSource>();
        public ReactiveProperty<Brush> ClockTextForeground { get; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> SecondLineColor { get; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> MinuteLineColor { get; } = new ReactiveProperty<Brush>();
        public ReactiveProperty<Brush> HourLineColor { get; } = new ReactiveProperty<Brush>();
        public ReactiveCommand<DragEventArgs> DropCommand { get; } = new ReactiveCommand<DragEventArgs>();
        public ReactiveCommand<DragEventArgs> PreviewDragOverCommand { get; } = new ReactiveCommand<DragEventArgs>();
        public ReactiveProperty<double> NowWidth { get; } = new ReactiveProperty<double>(500);
        public ReactiveProperty<double> NowHeight { get; } = new ReactiveProperty<double>(500);

        public MainWindowViewModels()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.PreviousFile))
            {
                Source.Value = new BitmapImage(new Uri(Properties.Settings.Default.PreviousFile, UriKind.Absolute));
            }

            var red = Properties.Settings.Default.ClockTextForeground_R;
            var green = Properties.Settings.Default.ClockTextForeground_G;
            var blue = Properties.Settings.Default.ClockTextForeground_B;
            ClockTextForeground.Value = new SolidColorBrush(Color.FromRgb((byte)(255 * red), (byte)(255 * green), (byte)(255 * blue)));

            red = Properties.Settings.Default.SecondLine_R;
            green = Properties.Settings.Default.SecondLine_G;
            blue = Properties.Settings.Default.SecondLine_B;
            SecondLineColor.Value = new SolidColorBrush(Color.FromRgb((byte)(255 * red), (byte)(255 * green), (byte)(255 * blue)));

            red = Properties.Settings.Default.MinuteLine_R;
            green = Properties.Settings.Default.MinuteLine_G;
            blue = Properties.Settings.Default.MinuteLine_B;
            MinuteLineColor.Value = new SolidColorBrush(Color.FromRgb((byte)(255 * red), (byte)(255 * green), (byte)(255 * blue)));

            red = Properties.Settings.Default.HourLine_R;
            green = Properties.Settings.Default.HourLine_G;
            blue = Properties.Settings.Default.HourLine_B;
            HourLineColor.Value = new SolidColorBrush(Color.FromRgb((byte)(255 * red), (byte)(255 * green), (byte)(255 * blue)));

            SettingCommand.Subscribe(x =>
            {
                var dialog = new SettingWindow();

                dialog.ShowDialog();

                var vm = dialog.DataContext as SettingWindowViewModel;

                if (vm.Result == false)
                {
                    return;
                }

                ClockTextForeground.Value = vm.TextSolidColorBrush.Value ?? new SolidColorBrush();
                SecondLineColor.Value = vm.SecondLineColorBrush.Value ?? new SolidColorBrush();
                MinuteLineColor.Value = vm.MinuteLineColorBrush.Value ?? new SolidColorBrush();
                HourLineColor.Value = vm.HourLineColorBrush.Value ?? new SolidColorBrush();

                Properties.Settings.Default.ClockTextForeground_R = vm.TextColorState.Value.RGB_R;
                Properties.Settings.Default.ClockTextForeground_G = vm.TextColorState.Value.RGB_G;
                Properties.Settings.Default.ClockTextForeground_B = vm.TextColorState.Value.RGB_B;
                Properties.Settings.Default.SecondLine_R = vm.SecondLineColorState.Value.RGB_R;
                Properties.Settings.Default.SecondLine_G = vm.SecondLineColorState.Value.RGB_G;
                Properties.Settings.Default.SecondLine_B = vm.SecondLineColorState.Value.RGB_B;
                Properties.Settings.Default.MinuteLine_R = vm.MinuteLineColorState.Value.RGB_R;
                Properties.Settings.Default.MinuteLine_G = vm.MinuteLineColorState.Value.RGB_G;
                Properties.Settings.Default.MinuteLine_B = vm.MinuteLineColorState.Value.RGB_B;
                Properties.Settings.Default.HourLine_R = vm.HourLineColorState.Value.RGB_R;
                Properties.Settings.Default.HourLine_G = vm.HourLineColorState.Value.RGB_G;
                Properties.Settings.Default.HourLine_B = vm.HourLineColorState.Value.RGB_B;
                Properties.Settings.Default.Save();
            });

            ExitCommand.Subscribe(x =>
            {
                Environment.Exit(0);
            });

            DropCommand.Subscribe(e =>
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                Source.Value = new BitmapImage(new Uri(files[0], UriKind.Absolute));

                Properties.Settings.Default.PreviousFile = files[0];
                Properties.Settings.Default.Save();
            });

            PreviewDragOverCommand.Subscribe(e =>
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
            });
        }
    }
}
