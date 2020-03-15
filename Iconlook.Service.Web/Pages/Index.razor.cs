using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Iconlook.Object;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using Agiper.Server;
using Microsoft.AspNetCore.Components;
using NServiceBus;
using Iconlook.Message;

namespace Iconlook.Service.Web.Pages
{
    public partial class Index
    {
        [Inject]
        public IMessageSession Endpoint { get; set; }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Endpoint.Send(new SendTelegramCommand
                {
                    Id = -1001449380420,
                    Text = "User Visited"
                }).ConfigureAwait(false);
                Endpoint.Publish(new WebAccessedEvent()).ConfigureAwait(false);
            }
            return base.OnAfterRenderAsync(firstRender);
        }

        protected override Task OnInitializedAsync()
        {
            using (var rolex = new Rolex())
            {
                using (var redis = ServerBase.Provider.GetService<IRedisClient>())
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