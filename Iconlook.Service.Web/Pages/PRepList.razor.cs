using Microsoft.AspNetCore.Components;
using Syncfusion.EJ2.Blazor.Navigations;

namespace Iconlook.Service.Web.Pages
{
    public partial class PRepList
    {
        protected TabHeader Summary;
        protected TabHeader Background;
        protected TabHeader Capability;
        protected TabHeader ScoreMatrix;
        protected TabHeader Contribution;
        protected TabHeader Transparency;
        protected TabAnimationSettings Animation;

        [Parameter]
        public int Size { get; set; }

        public bool Page { get; set; }

        [Parameter]
        public string Key { get; set; }

        public object Sizes { get; set; }

        protected override void OnInitialized()
        {
            Size = 22;
            Page = true;
            Sizes = new[] { 22, 50, 100 };
            Summary = new TabHeader { Text = "SUMMARY" };
            Background = new TabHeader { Text = "BACKGROUND" };
            Capability = new TabHeader { Text = "CAPABILITY" };
            ScoreMatrix = new TabHeader { Text = "SCORE MATRIX" };
            Contribution = new TabHeader { Text = "CONTRIBUTION" };
            Transparency = new TabHeader { Text = "TRANSPARENCY" };
            Animation = new TabAnimationSettings
            {
                Next = new TabAnimationNext { Duration = 0 },
                Previous = new TabAnimationPrevious { Duration = 0 }
            };
        }
    }
}