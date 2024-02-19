using Reactive.Bindings.Interactivity;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace AnalogClock.Converter
{
    public class EventArgsConverter : ReactiveConverter<EventArgs, EventArgs>
    {
        protected override IObservable<EventArgs?> OnConvert(IObservable<EventArgs?> source)
        {
            return source.Select(x => x);
        }
    }
}
