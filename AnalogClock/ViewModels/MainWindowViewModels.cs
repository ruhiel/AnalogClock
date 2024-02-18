﻿using AnalogClock.Views;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ReactiveCommand<DragEventArgs> DropCommand { get; } = new ReactiveCommand<DragEventArgs>();
        public ReactiveCommand<DragEventArgs> PreviewDragOverCommand { get; } = new ReactiveCommand<DragEventArgs>();

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

            SettingCommand.Subscribe(x =>
            {
                var dialog = new SettingWindow();

                var red = Properties.Settings.Default.ClockTextForeground_R;
                var green = Properties.Settings.Default.ClockTextForeground_G;
                var blue = Properties.Settings.Default.ClockTextForeground_B;

                var vm = dialog.DataContext as ColorPickerViewModel;

                if (vm == null)
                {
                    return;
                }

                var color = new ColorPicker.Models.ColorState();
                color.SetARGB(0, red, green, blue);

                vm.ColorState.Value = color;

                dialog.ShowDialog();

                if (vm.Result == false)
                {
                    return;
                }

                ClockTextForeground.Value = vm.SolidColorBrush.Value ?? new SolidColorBrush();

                Properties.Settings.Default.ClockTextForeground_R = vm.ColorState.Value.RGB_R;
                Properties.Settings.Default.ClockTextForeground_G = vm.ColorState.Value.RGB_G;
                Properties.Settings.Default.ClockTextForeground_B = vm.ColorState.Value.RGB_B;
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
