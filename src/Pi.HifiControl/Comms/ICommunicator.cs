using System;

namespace Pi.HifiControl.Comms
{
    public interface ICommunicator : IDisposable
    {
        void Send(Message input);

        void SendRaw(string input);

        IObservable<Message> ObservedMessages { get; }
    }
}
