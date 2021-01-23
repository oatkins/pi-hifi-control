using System.Text.RegularExpressions;
using static System.FormattableString;

namespace Pi.HifiControl.Comms
{
    public sealed record Message(int CommandGroup, int CommandNumber, string? CommandData = null)
    {
        public static Message? TryParse(string messageText)
        {
            var m = Regex.Match(messageText, @"^\#(?<group>\d+),(?<command>\d+)(,(?<data>.+))?$");
            if (m.Success)
            {
                return new Message(int.Parse(m.Groups["group"].Value), int.Parse(m.Groups["command"].Value), m.Groups["data"]?.Value);
            }

            return null;
        }

        public override string ToString() => 
            string.IsNullOrEmpty(CommandData) 
                ? Invariant($"#{CommandGroup:D2},{CommandNumber:D2}") 
                : Invariant($"#{CommandGroup:D2},{CommandNumber:D2},{CommandData}");
    }
}
