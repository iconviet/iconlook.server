using System;
using Mono.Unix;
using Mono.Unix.Native;

namespace Iconlook.Service.Api
{
    public class Program
    {
        public static void Main()
        {
            if (Type.GetType("Mono.Runtime") == null)
            {
                Console.ReadLine();
            }
            else
            {
                UnixSignal.WaitAny(new[]
                {
                    new UnixSignal(Signum.SIGINT),
                    new UnixSignal(Signum.SIGHUP),
                    new UnixSignal(Signum.SIGTERM),
                    new UnixSignal(Signum.SIGQUIT)
                });
            }
        }
    }
}