using System;
using Iconlook.Object;
using Iconlook.Service.Web.Sources;
using Syncfusion.EJ2.Blazor.Grids;

namespace Iconlook.Service.Web.Pages
{
    public partial class IndexBlockGrid : IDisposable
    {
        protected IDisposable Subscription;
        protected BlockResponse BlockModelType;
        protected EjsGrid<BlockResponse> BlockGrid;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            BlockModelType = new BlockResponse();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                var connection = Source.Blocks.Connect();
                Subscription = connection.Subscribe(changes =>
                {
                    foreach (var change in changes)
                    {
                        BlockGrid.AddRecord(change.Current);
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