using System;
using System.Linq;
using Iconlook.Service.Web.Sources;

namespace Iconlook.Service.Web.Views
{
    public partial class BlockHeight : IDisposable
    {
        protected long Height;
        protected IDisposable Subscription;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                var connection = Source.Blockchain.Connect();
                Subscription = connection.Subscribe(x =>
                {
                    Height = x.Last().Current.BlockHeight;
                    InvokeAsync(() => StateHasChanged());
                });
            }
        }

        public void Dispose()
        {
            Subscription?.Dispose();
        }
    }
}