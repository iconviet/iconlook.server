using System.Collections.Generic;
using Syncfusion.EJ2.Blazor.Navigations;

namespace Iconlook.Service.Web.Layouts
{
    public partial class MainLayout
    {
        protected List<MenuItem> MenuItems;

        protected override void OnInitialized()
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Who Are We" },
                        new MenuItem { Text = "Why Vote For Us" },
                        new MenuItem { Text = "Active Governance" },
                        new MenuItem { Text = "Reward Transparency" }
                    }
                },
                new MenuItem
                {
                    Text = "VOTERS", IconCss = "fal fa-person-sign", Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Who Should I Vote" },
                        new MenuItem { Text = "Why Should I Care" },
                        new MenuItem { Text = "How To Stake With Ledger *" },
                        new MenuItem { Text = "How To Stake With ICONex *" },
                        new MenuItem { Text = "How Much Reward Can I Get *" }
                    }
                },
                new MenuItem
                {
                    Text = "P-REPS", IconCss = "fal fa-users-class", Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Ranking", Url = "/preps" },
                        new MenuItem { Text = "Research" },
                        new MenuItem { Text = "Contribution" },
                        new MenuItem { Text = "Transactions *" },
                        new MenuItem { Text = "Alll Penalties *" }
                    }
                },
                new MenuItem
                {
                    Text = "D-APPS", Url = "/dapps", IconCss = "fal fa-rocket", Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "View Top 10 D-Apps *" },
                        new MenuItem { Text = "View D-Apps Transactions *" },
                        new MenuItem { Text = "View Exclusively Top Tokens *" }
                    }
                },
                new MenuItem
                {
                    Text = "BLOCKCHAIN", IconCss = "fal fa-chart-network", Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Unstaking Addresses", Url = "/addresses?state=unstaking" },
                        new MenuItem { Text = "Top 10 Addresses *" },
                        new MenuItem { Text = "Wellknown Addresses *" },
                        new MenuItem { Text = "Suspicious Transactions *" }
                    }
                },
                new MenuItem
                {
                    Text = "CALCULATOR", IconCss = "fal fa-calculator-alt", Items = new List<MenuItem>
                    {
                        new MenuItem { Text = "Staking Reward *" },
                        new MenuItem { Text = "Representative Reward *" }
                    }
                }
            };
        }
    }
}