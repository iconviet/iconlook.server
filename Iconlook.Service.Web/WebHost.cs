using Funq;
using Iconlook.Service.Api;

namespace Iconlook.Service.Web
{
    public class WebHost : ApiHost
    {
        public WebHost() : base("Web", typeof(WebHost).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            base.Configure(container);
            RegisterServicesInAssembly(typeof(ApiHost).Assembly);
        }
    }
}