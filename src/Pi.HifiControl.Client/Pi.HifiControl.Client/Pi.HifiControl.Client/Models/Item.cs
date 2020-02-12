namespace Pi.HifiControl.Client.Models
{
    public class Item : Model
    {
        private bool _power;
        private int _input;
        private int _volume;
        private bool _mute;

        public bool Power { get => _power; set => SetProperty(ref _power, value); }

        public int Input { get => _input; set => SetProperty(ref _input, value); }

        public int Volume { get => _volume; set => SetProperty(ref _volume, value); }

        public bool Mute { get => _mute; set => SetProperty(ref _mute, value); }
    }
}