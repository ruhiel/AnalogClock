using Reactive.Bindings.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

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
