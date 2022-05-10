namespace Pi.HifiControl.Comms
{
    public enum IRCommunicatorCode : byte
    {
        PowerToggle = 12,
        PowerOn = 14,
        PowerOff = 15,
        MuteToggle = 13,
        MuteOn = 50,
        MuteOff = 51,
        VolumeUp = 16,
        VolumeDown = 17,
        LcdBright = 18,
        BrightnessToggle = 72,
        LcdDim = 19,
        LcdOff = 71,
        SpeakerSelect = 20,
        SpeakerAB = 30,
        SpeakerA = 35,
        SpeakerB = 39,
        AnalogStereoDirect = 78,
        TriggerA = 82,
        TriggerB = 83,
        TriggerC = 84,
        SourceUp = 99,
        SourceDown = 126,
        Analog1 = 100,
        Analog2 = 101,
        Analog3 = 102,
        Analog4 = 103,
        Analog1Balanced = 104,
        Digital1 = 105,
        Digital2 = 106,
        Digital3 = 107,
        MP3 = 108,
        Usb = 114,
        Bluetooth = 115
    }

    public interface IIRCommunicator
    {
        void Send(IRCommunicatorCode code);
    }
}
