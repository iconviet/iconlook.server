using Syncfusion.EJ2.Blazor.Navigations;
using System.Collections.Generic;
using ItemModel = Syncfusion.EJ2.Blazor.SplitButtons.ItemModel;

namespace Iconlook.Service.Web.Pages
{
    public partial class Index
    {
        protected TabHeader Votes;
        protected TabHeader Blocks;
        protected TabHeader Production;
        protected TabHeader Governance;
        protected TabHeader Transactions;
        protected List<ItemModel> ToolItems;
        protected List<ItemModel> SearchItems;
        protected TabAnimationSettings Animation;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Animation = new TabAnimationSettings
            {
                Next = new TabAnimationNext { Duration = 0 },
                Previous = new TabAnimationPrevious { Duration = 0 }
            };
            ToolItems = new List<ItemModel>
            {
                new ItemModel { Text = "ROUND", IconCss = "fal fa-users-class" },
                new ItemModel { Text = "ANALYZE", IconCss = "fal fa-hashtag" },
                new ItemModel { Text = "INVESTIGATE", IconCss = "fal fa-dice" }
            };
            SearchItems = new List<ItemModel>
            {
                new ItemModel { Text = "P-REP", IconCss = "fal fa-users-class" },
                new ItemModel { Text = "ADDRESS", IconCss = "fal fa-hashtag" },
                new ItemModel { Text = "TRANSACTION", IconCss = "fal fa-dice" }
            };
            Votes = new TabHeader { Text = "VOTES", IconCss = "fal fa-ballot" };
            Blocks = new TabHeader { Text = "BLOCKS", IconCss = "fal fa-cube" };
            Production = new TabHeader { Text = "PRODUCTION", IconCss = "fal fa-server" };
            Transactions = new TabHeader { Text = "TRANSACTIONS", IconCss = "fal fa-repeat" };
            Governance = new TabHeader { Text = "GOVERNANCE", IconCss = "fal fa-users-class" };
        }
    }
}