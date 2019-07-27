using System.Reflection;
using Iconlook.Server;
using ServiceStack;

namespace Iconlook.Service
{
    public abstract class ServiceHost : Host
    {
        protected ServiceHost(string name, Assembly assembly) : base(name, assembly)
        {
        }

        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            Plugins.Add(new SessionFeature());
        }
    }
}