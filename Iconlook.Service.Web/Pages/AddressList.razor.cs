using System.Linq;
using Iconlook.Object;
using Iconlook.Server;
using Microsoft.AspNetCore.Components;
using ServiceStack.Redis;
using Syncfusion.EJ2.Blazor.Navigations;

namespace Iconlook.Service.Web.Pages
{
    public partial class AddressList
    {
        protected TabHeader Unstaking;
        protected TabHeader Undelegated;
        protected PeerResponse PeerResponse;
        protected ChainResponse ChainResponse;
        protected TabAnimationSettings Animation;

        [Parameter]
        public bool Paging { get; set; }

        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public bool Sorting { get; set; }

        [Parameter]
        public string State { get; set; }

        [Parameter]
        public object PageSizes { get; set; }

        protected override void OnInitialized()
        {
            PageSize = 20;
            Paging = false;
            Sorting = true;
            PageSizes = new[] { 20, 50, 100 };
            Unstaking = new TabHeader { Text = "UNSTAKING" };
            Undelegated = new TabHeader { Text = "UNDELEGATED" };
            Animation = new TabAnimationSettings
            {
                Next = new TabAnimationNext { Duration = 0 },
                Previous = new TabAnimationPrevious { Duration = 0 }
            };
            using (var redis = Host.Current.Resolve<IRedisClient>())
            {
                var peers = redis.As<PeerResponse>().GetAll();
                var chains = redis.As<ChainResponse>().GetAll();
                PeerResponse = peers.FirstOrDefault(x => x.State == "BlockGenerate");
                ChainResponse = chains.OrderByDescending(x => x.Timestamp).FirstOrDefault();
            }
        }
    }
}