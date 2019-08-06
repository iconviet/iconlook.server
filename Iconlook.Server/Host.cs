using System.Reflection;
using System.Threading.Tasks;
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
            LogEventLevel = LogEventLevel.Information;
        }
    }
}