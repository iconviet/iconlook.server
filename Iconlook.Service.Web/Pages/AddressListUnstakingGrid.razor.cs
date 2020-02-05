using Microsoft.AspNetCore.Components;

namespace Iconlook.Service.Web.Pages
{
    public partial class AddressListUnstakingGrid
    {
        [Parameter]
        public bool Paging { get; set; }

        [Parameter]
        public bool Sorting { get; set; }
        
        [Parameter]
        public int PageSize { get; set; }
        
        [Parameter]
        public object PageSizes { get; set; }
    }
}