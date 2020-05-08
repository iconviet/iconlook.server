using System;
using System.Linq;
using System.Threading.Tasks;
using Iconlook.Client;
using Iconlook.Client.Megaloop;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using Iconviet;
using Iconviet.Server;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Workers
{
    public class UpdateMegaloopWorker : WorkerBase
    {
        public override async Task StartAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Debug("{Work} started", nameof(UpdateMegaloopWorker));
                try
                {
                    var service = new MegaloopClient(Endpoints.TESTNET, 2);
                    var pool_size = await service.GetPoolSize();
                    var player_list = await service.GetPlayerList();
                    var megaloop = new MegaloopResponse
                    {
                        PoolSize = pool_size.ToIcxFromLoop(),
                        Players = player_list.GetKeys().ToList()
                    };
                    await Channel.Instance().Publish(new MegaloopUpdatedSignal { Megaloop = megaloop }).ConfigureAwait(false);
                    await Endpoint.Instance().Publish(new MegaloopUpdatedEvent { Megaloop = megaloop }).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error(exception, "{Work} failed to run. {Message}", nameof(UpdateMegaloopWorker), exception.Message);
                    }
                }
                Log.Debug("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdateMegaloopWorker), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}