using System.ComponentModel;
using System.Globalization;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using Pi.HifiControl.Comms;
using Serilog;

namespace Pi.HifiControl;

public enum AudioSource
{
    Analog1 = 0,
    Analog2 = 1,
    Analog3 = 2,
    Analog4 = 3,
    Digital1 = 4,
    Digital2 = 5,
    Digital3 = 6,
    Bluetooth = 14,
    Usb = 16,
    Analog1Balanced = 20
}

public class CambridgeCX81 : IDisposable, INotifyPropertyChanged
{
    private readonly ICommunicator _communicator;
    private readonly IRCommunicator _irCommunicator;
    private readonly CompositeDisposable _disposable = new();
    private readonly ILogger _logger;

    private bool _power;
    private bool _mute;
    private AudioSource _source;

    private bool _disposed;

    public CambridgeCX81(ICommunicator communicator, IRCommunicator irCommunicator, ILogger logger)
    {
        _communicator = communicator;
        _irCommunicator = irCommunicator;
        _disposable.Add(_communicator.ObservedMessages.Subscribe(m => this.Handle(m)));

        _logger = logger.ForContext<CambridgeCX81>();

        // Get power state.
        _communicator.Send(new Message(1, 1));

        // Get mute state.
        _communicator.Send(new Message(1, 3));

        // Get current source.
        _communicator.Send(new Message(3, 1));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public bool IsPoweredOn { get => _power; private set => SetProperty(ref _power, value); }

    public bool IsMuted { get => _mute; private set => SetProperty(ref _mute, value); }

    public AudioSource Source { get => _source; private set => SetProperty(ref _source, value); }

    public void PowerOn() => _communicator.Send(new Message(1, 2, "1"));

    public void PowerOff() => _communicator.Send(new Message(1, 2, "0"));

    public void Mute() => _communicator.Send(new Message(1, 4, "1"));

    public void Unmute() => _communicator.Send(new Message(1, 4, "0"));

    public void SetSource(AudioSource source) => _communicator.Send(new Message(3, 4, ((int)source).ToString(CultureInfo.InvariantCulture)));

    public void VolumeDown() => _irCommunicator.Send(IRCommunicatorCode.VolumeDown);

    public void VolumeUp() => _irCommunicator.Send(IRCommunicatorCode.VolumeUp);

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _communicator.Dispose();
        _disposable.Dispose();

        GC.SuppressFinalize(this);
    }

    private void Handle(Message message)
    {
        switch (message.CommandGroup)
        {
            case 0:
                // Errors.
                switch (message.CommandNumber)
                {
                    case 1:
                        _logger.Error("Command group unknown");
                        break;
                    case 2:
                        _logger.Error("Command number unknown");
                        break;
                    case 3:
                        _logger.Error("Command data error");
                        break;
                    case 4:
                        _logger.Error("Command not available");
                        break;
                }

                break;

            case 2:
                // Replies to amplifier commands.
                switch (message.CommandNumber)
                {
                    case 1:
                        IsPoweredOn = message.CommandData == "1";
                        _logger.Information("Power state changed to {PowerState}", IsPoweredOn);
                        break;
                    case 3:
                        IsMuted = message.CommandData == "1";
                        _logger.Information("Mute state changed to {MuteState}", IsMuted);
                        break;
                }

                break;

            case 4:
                // Replies to source commands.
                switch (message.CommandNumber)
                {
                    case 1:
                        if (int.TryParse(message.CommandData, out var newSource))
                        {
                            Source = (AudioSource)newSource;
                            _logger.Information("Source changed to {Source}", Source);
                        }

                        break;
                }

                break;

            default:
                _logger.Information("Unknown message: {Message}", message.ToString());
                break;
        }
    }

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        return false;
    }
}
