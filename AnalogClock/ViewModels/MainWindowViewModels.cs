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
        public ReactiveCommand<EventArgs> ExitCommand { get; } = new ReactiveCommand<EventArgs>();
        public ReactiveProperty<ImageSource> Source { get; } = new ReactiveProperty<ImageSource>();
        public ReactiveCommand<DragEventArgs> DropCommand { get; } = new ReactiveCommand<DragEventArgs>();
        public ReactiveCommand<DragEventArgs> PreviewDragOverCommand { get; } = new ReactiveCommand<DragEventArgs>();

        public MainWindowViewModels()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.PreviousFile))
            {
                Source.Value = new BitmapImage(new Uri(Properties.Settings.Default.PreviousFile, UriKind.Absolute));
            }
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
