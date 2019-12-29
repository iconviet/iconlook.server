using Iconlook.Object;
using Microsoft.AspNetCore.Components;
using Syncfusion.EJ2.Blazor.Navigations;

namespace Iconlook.Service.Web.Pages
{
    public partial class AddressList
    {
        protected TabHeader Unstaking;
        protected PeerResponse PeerResponse;
        protected ChainResponse ChainResponse;
        protected TabAnimationSettings Animation;

        [Parameter]
        public int Size { get; set; }

        public bool Page { get; set; }

        public object Sizes { get; set; }

        [Parameter]
        public string State { get; set; }

        protected override void OnInitialized()
        {
            Size = 200;
            Sizes = new[] { 22, 50, 100 };
            Unstaking = new TabHeader { Text = "UNSTAKING" };
            Animation = new TabAnimationSettings
            {
                Next = new TabAnimationNext { Duration = 0 },
                Previous = new TabAnimationPrevious { Duration = 0 }
            };
        }
    }
}