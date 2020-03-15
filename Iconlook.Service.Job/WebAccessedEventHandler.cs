using System.Threading.Tasks;
using Agiper.Server;
using NServiceBus;

namespace Iconlook.Service.Job
{
    public class WebAccessedEventHandler : HandlerBase, IHandleMessages<WebAccessedEventHandler>
    {
        public Task Handle(WebAccessedEventHandler message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}