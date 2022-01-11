using System.Reactive.Linq;
using System.Text;

namespace Pi.HifiControl.Comms;

public static class MessageAggregator
{
    public static IObservable<string> ObserveTerminatedStrings(this IObservable<string> dataBatches) =>
        Observable.Create<string>(
            observer =>
            {
                var b = new StringBuilder();

                return dataBatches.Subscribe(
                    d =>
                    {
                        var incoming = d.AsSpan();

                        int term = incoming.IndexOf('\r');
                        while (term >= 0)
                        {
                            var line = incoming.Slice(0, term);
                            b.Append(line);

                            Push();

                            incoming = incoming.Slice(term + 1);
                            term = incoming.IndexOf('\r');
                        }

                        b.Append(incoming);
                    },
                    ex =>
                    {
                        Push();
                        observer.OnError(ex);
                    },
                    () =>
                    {
                        Push();
                        observer.OnCompleted();
                    });

                void Push()
                {
                    if (b.Length > 0)
                    {
                        observer.OnNext(b.ToString());
                        b.Clear();
                    }
                }
            });
}
