using System.IO.Ports;
using System.Reactive.Linq;
using Serilog;

namespace Pi.HifiControl.Comms;

public sealed class SerialCommunicator : ICommunicator, IDisposable
{
    private static readonly ILogger Logger = Log.ForContext<SerialCommunicator>();

    private readonly SerialPort _port;

    public SerialCommunicator()
    {
        _port = new SerialPort("/dev/serial0", 9600, Parity.None, 8, StopBits.One)
        {
            NewLine = "\r"
        };

        _port.Open();

        var rawMessages = Observable.FromEventPattern<SerialDataReceivedEventHandler, SerialDataReceivedEventArgs>(h => _port.DataReceived += h, h => _port.DataReceived -= h)
            .TakeWhile(x => x.EventArgs.EventType == SerialData.Chars)
            .Select(_ => _port.ReadExisting());

        var parsed = from m in rawMessages.ObserveTerminatedStrings()
                     let p = Message.TryParse(m)
                     select (Raw: m, Parsed: p);

        ObservedMessages = from p in parsed.Do(x => WriteLog(x.Raw, x.Parsed))
                           where p.Parsed != null
                           select p.Parsed;
    }

    public IObservable<Message> ObservedMessages { get; }

    public void Dispose()
    {
        _port.Dispose();
    }

    public void Send(Message input)
    {
        string m = input.ToString();
        Logger.Verbose("Sending message {RawMessage}", m);
        _port.WriteLine(m);
    }

    public void SendRaw(string input)
    {
        Log.Verbose("Sending raw message {RawMessage}", input);
        _port.WriteLine(input);
    }

    private void WriteLog(string rawMessage, Message parsedMessage)
    {
        if (parsedMessage == null)
        {
            Logger.Warning("Failed to parse message {RawMessage}", rawMessage);
        }
        else
        {
            Logger.Verbose("Received message {RawMessage}", rawMessage);
        }
    }
}
