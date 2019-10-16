using Microsoft.AspNetCore.Components;

namespace Iconlook.Service.Web.Pages
{
    public partial class Transaction
    {
        [Parameter]
        public string Hash { get; set; }
    }
}