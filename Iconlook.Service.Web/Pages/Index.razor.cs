using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Message;
using Iconlook.Object;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using Serilog;
using ServiceStack.Redis;

namespace Iconlook.Service.Web.Pages
{
    public partial class Index
    {
        [Inject]
        public IMessageSession Endpoint { get; set; }

        [Inject]
        public HttpContextAccessor HttpAccessor { get; set; }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Endpoint.Publish(new UserTrackedEvent
                {
                    Description = "User Visited"
                }).ConfigureAwait(false);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected override Task OnInitializedAsync()
        {
            using (var rolex = new Rolex())
            {
                using (var redis = HostBase.Container.GetService<IRedisClient>())
                {
                    var peers = redis.As<PeerResponse>().GetAll();
                    var chains = redis.As<ChainResponse>().GetAll();
                    PeerResponse = peers.FirstOrDefault(x => x.State == "BlockGenerate");
                    ChainResponse = chains.OrderByDescending(x => x.Timestamp).FirstOrDefault();
                    if (rolex.Elapsed.Milliseconds > 500)
                    {
                        Log.Warning("{Peer} peer and {Chain} chain loaded in {Elapsed}ms", peers.Count, chains.Count, rolex.Elapsed.TotalMilliseconds);
                    }
                }
            }
            return base.OnInitializedAsync();
        }
    }
}