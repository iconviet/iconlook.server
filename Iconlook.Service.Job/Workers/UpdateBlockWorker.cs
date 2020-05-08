using System;
using System.Linq;
using System.Threading.Tasks;
using Iconlook.Client;
using Iconlook.Client.Service;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using Iconviet;
using Iconviet.Server;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Workers
{
    public class UpdateBlockWorker : WorkerBase
    {
        public static long LastBlockHeight;

        private const string EMPTY_ADDRESS = "cx0000000000000000000000000000000000000000";

        public override async Task StartAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Debug("{Work} started", nameof(UpdateBlockWorker));
                try
                {
                    var service = new IconServiceClient(2);
                    var last_block = await service.GetLastBlock();
                    if (last_block != null)
                    {
                        if (last_block.GetHeight() > LastBlockHeight)
                        {
                            var transactions = last_block.GetTransactions().Select(x => new TransactionResponse
                            {
                                Id = x.GetTxHash().ToString(),
                                Hash = x.GetTxHash().ToString(),
                                Block = (long) last_block.GetHeight(),
                                To = x.GetTo()?.ToString() ?? EMPTY_ADDRESS,
                                From = x.GetFrom()?.ToString() ?? EMPTY_ADDRESS,
                                Timestamp = x.GetTimestamp().Value.ToDateTimeOffset(),
                                Fee = x.GetFee().HasValue ? x.GetFee().Value.ToIcxFromLoop() : 0,
                                Amount = x.GetValue().HasValue ? x.GetValue().Value.ToIcxFromLoop() : 0
                            }).ToList();
                            var block = new BlockResponse
                            {
                                PeerId = last_block.GetPeerId(),
                                Id = (long) last_block.GetHeight(),
                                Fee = transactions.Sum(x => x.Fee),
                                TransactionCount = transactions.Count,
                                Hash = last_block.GetBlockHash().ToString(),
                                TotalAmount = transactions.Sum(x => x.Amount),
                                PrevHash = last_block.GetPrevBlockHash().ToString(),
                                Height = LastBlockHeight = (long) last_block.GetHeight(),
                                Timestamp = last_block.GetTimestamp().ToDateTimeOffset()
                            };
                            await Channel.Instance().Publish(new BlockUpdatedSignal
                            {
                                Block = block,
                                Transactions = transactions
                            }).ConfigureAwait(false);
                            await Endpoint.Instance().Publish(new BlockUpdatedEvent
                            {
                                Block = block,
                                Transactions = transactions
                            }).ConfigureAwait(false);
                            await Task.Run(() =>
                            {
                                using (var redis = Redis.Instance())
                                {
                                    redis.As<BlockResponse>().Store(block, TimeSpan.FromSeconds(60));
                                    transactions.ForEach(x => redis.As<TransactionResponse>().Store(x, TimeSpan.FromSeconds(60)));
                                }
                            }).ConfigureAwait(false);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error(exception, "{Work} failed to run. {Message}", nameof(UpdateBlockWorker), exception.Message);
                    }
                }
                Log.Debug("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdateBlockWorker), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}