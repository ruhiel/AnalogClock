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
        }
    }
}
