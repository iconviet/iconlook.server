using System.Reflection;
using Agiper;
using Agiper.Server;
using Serilog.Events;

namespace Iconlook.Server
{
    public abstract class Host : HostBase
    {
        public override string ProjectName => "Iconlook";

        public static HostBase Current => Instance as HostBase;

        protected Host(string name, Assembly assembly) : base(name, assembly)
        {
            if (Environment == Environment.Localhost)
            {
                LogEventLevel = LogEventLevel.Verbose;
                NServiceBusTransport = NServiceBusTransport.Learning;
                NServiceBusPersistence = NServiceBusPersistence.Memory;
            }
            else
            {
                LogEventLevel = LogEventLevel.Information;
                NServiceBusTransport = NServiceBusTransport.RabbitMQ;
                NServiceBusPersistence = NServiceBusPersistence.SqlServer;
            }
        }
    }
}