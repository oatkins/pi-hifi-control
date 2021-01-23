using System;
using System.Linq;
using Pi.HifiControl.Comms;
using Serilog;
using Serilog.Events;

namespace Pi.HifiControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(LogEventLevel.Verbose)
                .CreateLogger();

            using var communicator = new SerialCommunicator();
            using var amp = new CambridgeCX81(communicator, Log.Logger);

            if (args?.Length > 0)
            {
                HandleCommands(communicator, amp, args);

                if (args.Contains("--wait"))
                {
                    string? q;
                    do
                    {
                        q = Console.ReadLine();
                    }
                    while (q != "q");
                }
            }
            else
            {
                string? s = Console.ReadLine();
                while (!string.IsNullOrEmpty(s))
                {
                    HandleCommands(communicator, amp, s.Split(" "));
                    s = Console.ReadLine();
                }
            }

            Console.WriteLine("Done!");
        }

        private static void HandleCommands(ICommunicator com, CambridgeCX81 amp, string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0].ToLowerInvariant())
                {
                    case "on":
                        amp.PowerOn();
                        break;
                    case "off":
                        amp.PowerOff();
                        break;
                    case "mute":
                        amp.Mute();
                        break;
                    case "unmute":
                        amp.Unmute();
                        break;
                    case "source" when args.Length > 1:
                        amp.SetSource(Enum.Parse<AudioSource>(args[1]));
                        break;
                    default:
                        if (args.Length == 1)
                        {
                            com.SendRaw(args[0]);
                        }

                        break;
                }
            }
        }
    }
}
