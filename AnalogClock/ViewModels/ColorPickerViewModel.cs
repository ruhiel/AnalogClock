using AnalogClock.Properties;
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
    public class ColorPickerViewModel
    {
        public bool Result { get; private set; }
        public ReactiveProperty<ColorState> ColorState { get; set; } = new ReactiveProperty<ColorState>();
        public ReadOnlyReactiveProperty<SolidColorBrush?> SolidColorBrush { get; set; }
        public ReactiveCommand CloseWindow { get; } = new ReactiveCommand();
        public ReactiveCommand CloseWindowCancel { get; } = new ReactiveCommand();

        public ColorPickerViewModel()
        {
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

            SolidColorBrush = ColorState.Select(x =>
            {
                var red = 255 * x.RGB_R;
                var green = 255 * x.RGB_G;
                var blue = 255 * x.RGB_B;

                return new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
            }).ToReadOnlyReactiveProperty();
        }
    }
}
