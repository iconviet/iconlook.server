using Microsoft.AspNetCore.Components;

namespace Iconlook.Service.Web.Pages
{
    public partial class Block
    {
        [Parameter]
        public int Height { get; set; }

        [Parameter]
        public string Hash { get; set; }
    }
}