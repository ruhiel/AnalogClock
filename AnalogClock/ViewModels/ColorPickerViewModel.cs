using ColorPicker.Models;
using Reactive.Bindings;
using System;
using System.Reactive.Linq;

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
