using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Iconlook.Object;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using Agiper.Server;

namespace Iconlook.Service.Web.Pages
{
    public partial class Index
    {
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