using System.Reflection;
using Agiper.Server;

namespace Iconlook.Server
{
    public abstract class Host : HostBase
    {
        public override string ProjectName => "Iconlook";

        public static HostBase Current => Instance as HostBase;

        protected Host(string name, Assembly assembly) : base(name, assembly)
        {
            NServiceBusTransport = NServiceBusTransport.RabbitMQ;
            HangfireJobPersistence = HangfireJobPersistence.Memory; // TODO: change to Redis
            NServiceBusPersistence = NServiceBusPersistence.SqlServer;
        }
    }
}