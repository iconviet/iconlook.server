using Iconlook.Object;
using Iconlook.Service.Web.Sources;
using Syncfusion.EJ2.Blazor.Grids;
using System;

namespace Iconlook.Service.Web.Pages
{
    public partial class IndexTransactionGrid : IDisposable
    {
        protected IDisposable Subscription;

        protected EjsGrid<TransactionResponse> TransactionGrid;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                var connection = Source.Transactions.Connect();
                Subscription = connection.Subscribe(changes =>
                {
                    foreach (var change in changes)
                    {
                        TransactionGrid.AddRecord(change.Current);
                    }
                });
            }
        }

        public void Dispose()
        {
            Subscription.Dispose();
        }
    }
}