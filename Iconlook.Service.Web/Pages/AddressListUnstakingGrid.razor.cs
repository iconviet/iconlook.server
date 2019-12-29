using Microsoft.AspNetCore.Components;

namespace Iconlook.Service.Web.Pages
{
    public partial class AddressListUnstakingGrid
    {
        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public bool CanPage { get; set; }

        [Parameter]
        public object PageSizes { get; set; }
    }
}