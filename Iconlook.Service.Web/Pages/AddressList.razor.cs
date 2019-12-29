using Iconlook.Object;
using Microsoft.AspNetCore.Components;

namespace Iconlook.Service.Web.Pages
{
    public partial class AddressList
    {
        protected PeerResponse PeerResponse;
        protected ChainResponse ChainResponse;
        
        [Parameter]
        public string State { get; set; }
    }
}