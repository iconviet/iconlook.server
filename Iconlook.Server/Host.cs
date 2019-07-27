using System.Reflection;
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
            LogEventLevel = LogEventLevel.Information;
            NServiceBusTransport = NServiceBusTransport.RabbitMQ;
            NServiceBusPersistence = NServiceBusPersistence.SqlServer;
        }
    }
}