using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Object;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;

namespace Iconlook.Service.Web.Pages
{
    public partial class AddressList
    {
        protected override Task OnInitializedAsync()
        {
            
            using (var redis = ServerBase.Provider.GetService<IRedisClient>())
            {
                var peers = redis.As<PeerResponse>().GetAll();
                var chains = redis.As<ChainResponse>().GetAll();
                PeerResponse = peers.FirstOrDefault(x => x.State == "BlockGenerate");
                ChainResponse = chains.OrderByDescending(x => x.Timestamp).FirstOrDefault();
            }
            return base.OnInitializedAsync();
        }
    }
}