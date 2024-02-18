using AnalogClock.Views;
using ColorPicker.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AnalogClock.ViewModels
{
    public class SettingWindowViewModel
    {
        public bool Result { get; private set; }
        public ReactiveProperty<ColorState> TextColorState { get; set; } = new ReactiveProperty<ColorState>();
        public ReadOnlyReactiveProperty<SolidColorBrush?> TextSolidColorBrush { get; set; }
        public ReactiveProperty<ColorState> SecondLineColorState { get; set; } = new ReactiveProperty<ColorState>();
        public ReadOnlyReactiveProperty<SolidColorBrush?> SecondLineColorBrush { get; set; }
        public ReactiveProperty<ColorState> MinuteLineColorState { get; set; } = new ReactiveProperty<ColorState>();
        public ReadOnlyReactiveProperty<SolidColorBrush?> MinuteLineColorBrush { get; set; }
        public ReactiveProperty<ColorState> HourLineColorState { get; set; } = new ReactiveProperty<ColorState>();
        public ReadOnlyReactiveProperty<SolidColorBrush?> HourLineColorBrush { get; set; }
        public ReactiveCommand<EventArgs> TextSettingCommand { get; } = new ReactiveCommand<EventArgs>();
        public ReactiveCommand<EventArgs> SecondLineSettingCommand { get; } = new ReactiveCommand<EventArgs>();
        public ReactiveCommand<EventArgs> MinuteLineSettingCommand { get; } = new ReactiveCommand<EventArgs>();
        public ReactiveCommand<EventArgs> HourLineSettingCommand { get; } = new ReactiveCommand<EventArgs>();
        public ReactiveCommand CloseWindow { get; } = new ReactiveCommand();
        public ReactiveCommand CloseWindowCancel { get; } = new ReactiveCommand();
        public SettingWindowViewModel()
        {
            var red = Properties.Settings.Default.ClockTextForeground_R;
            var green = Properties.Settings.Default.ClockTextForeground_G;
            var blue = Properties.Settings.Default.ClockTextForeground_B;
            var color = new ColorState();
            color.SetARGB(0, red, green, blue);

            TextColorState.Value = color;

            red = Properties.Settings.Default.SecondLine_R;
            green = Properties.Settings.Default.SecondLine_G;
            blue = Properties.Settings.Default.SecondLine_B;
            color = new ColorState();
            color.SetARGB(0, red, green, blue);

            SecondLineColorState.Value = color;

            red = Properties.Settings.Default.MinuteLine_R;
            green = Properties.Settings.Default.MinuteLine_G;
            blue = Properties.Settings.Default.MinuteLine_B;
            color = new ColorState();
            color.SetARGB(0, red, green, blue);

            MinuteLineColorState.Value = color;

            green = Properties.Settings.Default.HourLine_R;
            green = Properties.Settings.Default.HourLine_G;
            blue = Properties.Settings.Default.HourLine_B;
            color = new ColorState();
            color.SetARGB(0, red, green, blue);

            HourLineColorState.Value = color;

            TextSettingCommand.Subscribe(x =>
            {
                var dialog = new ColorPickerWindow();

                var vm = dialog.DataContext as ColorPickerViewModel;

                if (vm == null)
                {
                    return;
                }

                vm.ColorState.Value = TextColorState.Value;

                dialog.ShowDialog();

                if (vm.Result == false)
                {
                    return;
                }

                TextColorState.Value = vm.ColorState.Value;
            });

            SecondLineSettingCommand.Subscribe(x =>
            {
                var dialog = new ColorPickerWindow();

                var vm = dialog.DataContext as ColorPickerViewModel;

                if (vm == null)
                {
                    return;
                }

                vm.ColorState.Value = SecondLineColorState.Value;

                dialog.ShowDialog();

                if (vm.Result == false)
                {
                    return;
                }

                SecondLineColorState.Value = vm.ColorState.Value;
            });

            MinuteLineSettingCommand.Subscribe(x =>
            {
                var dialog = new ColorPickerWindow();

                var vm = dialog.DataContext as ColorPickerViewModel;

                if (vm == null)
                {
                    return;
                }

                vm.ColorState.Value = MinuteLineColorState.Value;

                dialog.ShowDialog();

                if (vm.Result == false)
                {
                    return;
                }

                MinuteLineColorState.Value = vm.ColorState.Value;
            });

            HourLineSettingCommand.Subscribe(x =>
            {
                var dialog = new ColorPickerWindow();

                var vm = dialog.DataContext as ColorPickerViewModel;

                if (vm == null)
                {
                    return;
                }

                vm.ColorState.Value = HourLineColorState.Value;

                dialog.ShowDialog();

                if (vm.Result == false)
                {
                    return;
                }

                HourLineColorState.Value = vm.ColorState.Value;
            });

            TextSolidColorBrush = TextColorState.Select(x =>
            {
                var red = 255 * x.RGB_R;
                var green = 255 * x.RGB_G;
                var blue = 255 * x.RGB_B;

                return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
            }).ToReadOnlyReactiveProperty();

            SecondLineColorBrush = SecondLineColorState.Select(x =>
            {
                var red = 255 * x.RGB_R;
                var green = 255 * x.RGB_G;
                var blue = 255 * x.RGB_B;

                return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
            }).ToReadOnlyReactiveProperty();

            MinuteLineColorBrush = MinuteLineColorState.Select(x =>
            {
                var red = 255 * x.RGB_R;
                var green = 255 * x.RGB_G;
                var blue = 255 * x.RGB_B;

                return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
            }).ToReadOnlyReactiveProperty();

            HourLineColorBrush = HourLineColorState.Select(x =>
            {
                var red = 255 * x.RGB_R;
                var green = 255 * x.RGB_G;
                var blue = 255 * x.RGB_B;

                return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
            }).ToReadOnlyReactiveProperty();

            CloseWindow.Subscribe(x =>
            {
                ((System.Windows.Window)x).Close();

                Result = true;
            });

            CloseWindowCancel.Subscribe(x =>
            {
                ((System.Windows.Window)x).Close();

                Result = false;
            });
        }
    }
}
