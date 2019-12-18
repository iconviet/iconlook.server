using System.Collections.Generic;
using Syncfusion.EJ2.Blazor.Navigations;
using Syncfusion.EJ2.Blazor.SplitButtons;

namespace Iconlook.Service.Web.Pages
{
    public partial class Index
    {
        protected TabHeader Votes;
        protected TabHeader Blocks;
        protected TabHeader Production;
        protected TabHeader Governance;
        protected TabHeader Transactions;
        protected TabAnimationSettings Animation;
        protected List<DropDownButtonItem> ToolItems;
        protected List<DropDownButtonItem> SearchItems;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Animation = new TabAnimationSettings
            {
                Next = new TabAnimationNext { Duration = 0 },
                Previous = new TabAnimationPrevious { Duration = 0 }
            };
            ToolItems = new List<DropDownButtonItem>
            {
                new DropDownButtonItem { Text = "ROUND", IconCss = "fal fa-users-class" },
                new DropDownButtonItem { Text = "ANALYZE", IconCss = "fal fa-hashtag" },
                new DropDownButtonItem { Text = "INVESTIGATE", IconCss = "fal fa-dice" }
            };
            SearchItems = new List<DropDownButtonItem>
            {
                new DropDownButtonItem { Text = "P-REP", IconCss = "fal fa-users-class" },
                new DropDownButtonItem { Text = "ADDRESS", IconCss = "fal fa-hashtag" },
                new DropDownButtonItem { Text = "TRANSACTION", IconCss = "fal fa-dice" }
            };
            Votes = new TabHeader { Text = "VOTES", IconCss = "fal fa-ballot" };
            Blocks = new TabHeader { Text = "BLOCKS", IconCss = "fal fa-cube" };
            Production = new TabHeader { Text = "PRODUCTION", IconCss = "fal fa-server" };
            Transactions = new TabHeader { Text = "TRANSACTIONS", IconCss = "fal fa-repeat" };
            Governance = new TabHeader { Text = "GOVERNANCE", IconCss = "fal fa-users-class" };
        }
    }
}