using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Client.Service;
using Iconlook.Client.Tracker;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using NServiceBus;
using ServiceStack.OrmLite;

namespace Iconlook.Service.Job.Blockchain
{
    public class UpdateBlockchainJob : JobBase
    {
        private static readonly IconServiceClient Service = new IconServiceClient();
        private static readonly IconTrackerClient Tracker = new IconTrackerClient();

        public override async Task RunAsync()
        {
            try
            {
                var main_info = await Tracker.GetMainInfo();
                var iiss_info = await Service.GetIissInfo();
                var prep_info = await Service.GetPRepInfo();
                var last_block = await Service.GetLastBlock();
                var total_supply = await Service.GetTotalSupply();
                var transactions = last_block.GetTransactions().Select(x => new Transaction
                {
                    To = x.GetTo()?.ToString(),
                    From = x.GetFrom()?.ToString(),
                    Hash = x.GetTxHash()?.ToString(),
                    Block = (long) last_block.GetHeight(),
                    Fee = x.GetFee().HasValue ? x.GetFee().Value.ToIcxFromLoop() : 0,
                    Amount = x.GetValue().HasValue ? x.GetValue().Value.ToIcxFromLoop() : 0,
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(x.GetTimestamp().Value.ToMilliseconds())
                }).ToList();
                var block = new Block
                {
                    PeerId = last_block.GetPeerId(),
                    Transactions = transactions.Count,
                    Fee = transactions.Sum(x => x.Fee),
                    Height = (long) last_block.GetHeight(),
                    Amount = transactions.Sum(x => x.Amount),
                    Hash = last_block.GetBlockHash()?.ToString(),
                    PrevHash = last_block.GetPrevBlockHash()?.ToString(),
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(last_block.GetTimestamp().ToMilliseconds())
                };
                await Channel.Publish(new BlockProducedSignal
                {
                    Block = block.ToResponse()
                });
                await Endpoint.Publish(new BlockProducedEvent
                {
                    Block = block,
                    Transactions = transactions
                });
                await Endpoint.Publish(new BlockchainUpdatedEvent
                {
                    BlockHeight = block.Height,
                    Timestamp = block.Timestamp,
                    IcxSupply = (long) total_supply.ToIcxFromLoop()
                });
                await Channel.Publish(new BlockchainUpdatedSignal
                {
                    Blockchain = new BlockchainResponse
                    {
                        Timestamp = block.Timestamp,
                        MarketCap = (long) main_info.GetMarketCap(),
                        IcxSupply = (long) main_info.GetIcxSupply(),
                        BlockHeight = (long) iiss_info.GetBlockHeight(),
                        IcxCirculation = (long) main_info.GetIcxCirculation(),
                        PublicTreasury = (long) main_info.GetPublicTreasury(),
                        TransactionCount = (long) main_info.GetTransactionCount(),
                        TotalStaked = (long) prep_info.GetTotalStaked().ToIcxFromLoop(),
                        TotalDelegated = (long) prep_info.GetTotalDelegated().ToIcxFromLoop()
                    }
                });
                await Task.Run(async () =>
                {
                    var db = Db.Instance();
                    if (!db.Exists<Block>(x => x.Height == block.Height))
                    {
                        await db.InsertAsync(block);
                        // await db.InsertAllAsync(transactions);
                        var block_redis = Redis.Instance().As<BlockResponse>();
                        block_redis.Store(block.ToResponse(), TimeSpan.FromMinutes(1));
                        var transaction_redis = Redis.Instance().As<TransactionResponse>();
                        transactions.ForEach(x => transaction_redis.Store(x.ToResponse(), TimeSpan.FromMinutes(1)));
                    }
                });
            }
            catch
            {
            }
        }
    }
}