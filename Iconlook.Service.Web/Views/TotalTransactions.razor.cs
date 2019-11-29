using System;
using System.Linq;
using Iconlook.Service.Web.Sources;

namespace Iconlook.Service.Web.Views
{
    public partial class TotalTransactions : IDisposable
    {
        protected long Count;

        protected IDisposable Subscription;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                var connection = Source.Blockchain.Connect();
                Subscription = connection.Subscribe(x =>
                {
                    Count = x.Last().Current.TotalTransactions;
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