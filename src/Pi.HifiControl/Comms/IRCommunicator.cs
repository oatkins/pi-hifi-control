// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using RaspberryIRDotNet.PacketFormats.RC5;
using RaspberryIRDotNet.TX;
using Serilog;

namespace Pi.HifiControl.Comms
{
    public class IRCommunicator : IIRCommunicator, IDisposable
    {
        private static readonly ILogger Logger = Log.ForContext<IRCommunicator>();

        private static readonly byte CXA81SystemCode = 16;

        private PulseSpaceTransmitter_ManualOpenClose _irTransmitter;
        private RC5Converter _irEncoder;

        private bool _irToggle;

        public IRCommunicator()
        {
            _irTransmitter = new PulseSpaceTransmitter_ManualOpenClose
            {
                DutyCycle = 25,
                Frequency = 36000,
                TransmissionDevice = @"/dev/lirc0"
            };

            _irEncoder = new RC5Converter();
            _irTransmitter.Open();
        }

        public void Dispose() => _irTransmitter.Dispose();

        public void Send(IRCommunicatorCode code)
        {
            var packet = new RC5BasicPacket { Address = CXA81SystemCode, Toggle = _irToggle, Command = (byte)code };
            _irToggle = !_irToggle;

            Logger.Verbose($"Sending IR signal {packet.Command}");

            var encoded = _irEncoder.ToIR(packet);
            _irTransmitter.Send(new RaspberryIRDotNet.IRPulseMessage(encoded, 1000));
        }
    }
}
