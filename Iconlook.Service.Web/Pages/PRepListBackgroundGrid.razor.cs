using Microsoft.AspNetCore.Components;

namespace Iconlook.Service.Web.Pages
{
    public partial class PRepListBackgroundGrid
    {
        [Parameter]
        public int PageSize { get; set; }

        [Parameter]
        public bool CanPage { get; set; }

        [Parameter]
        public string EditKey { get; set; }

        [Parameter]
        public object PageSizes { get; set; }

        public bool CanEdit => !string.IsNullOrWhiteSpace(EditKey);
    }
}