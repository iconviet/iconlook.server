using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Client.Service;
using Iconlook.Client.Tracker;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using NServiceBus;

namespace Iconlook.Service.Job
{
    public class UpdateBlockchainJob : JobBase
    {
        private const string EMPTY_ADDRESS = "cx0000000000000000000000000000000000000000";

        public override async Task RunAsync()
        {
            try
            {
                var service = new IconServiceClient();
                var tracker = new IconTrackerClient();
                var main_info = await tracker.GetMainInfo();
                var iiss_info = await service.GetIissInfo();
                var prep_info = await service.GetPRepInfo();
                var last_block = await service.GetLastBlock();
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
                    Height = (long) last_block.GetHeight(),
                    Hash = last_block.GetBlockHash().ToString(),
                    TotalAmount = transactions.Sum(x => x.Amount),
                    PrevHash = last_block.GetPrevBlockHash().ToString(),
                    Timestamp = last_block.GetTimestamp().ToDateTimeOffset()
                };
                var chain = new ChainResponse
                {
                    MarketCap = (long) main_info.GetMarketCap(),
                    IcxSupply = (long) main_info.GetIcxSupply(),
                    BlockHeight = (long) iiss_info.GetBlockHeight(),
                    IcxCirculation = (long) main_info.GetIcxCirculation(),
                    PublicTreasury = (long) main_info.GetPublicTreasury(),
                    Timestamp = last_block.GetTimestamp().ToDateTimeOffset(),
                    TransactionCount = (long) main_info.GetTransactionCount(),
                    TotalStaked = (long) prep_info.GetTotalStaked().ToIcxFromLoop(),
                    TotalDelegated = (long) prep_info.GetTotalDelegated().ToIcxFromLoop()
                };
                await Channel.Publish(new BlockProducedSignal
                {
                    Block = block,
                    Transactions = transactions
                });
                await Endpoint.Publish(new BlockProducedEvent
                {
                    Block = block,
                    Transactions = transactions
                });
                await Channel.Publish(new ChainUpdatedSignal { Chain = chain });
                await Endpoint.Publish(new ChainUpdatedEvent { Chain = chain });
                await Task.Run(() =>
                {
                    using (var redis = Redis.Instance())
                    {
                        redis.As<BlockResponse>().Store(block, TimeSpan.FromMinutes(2));
                        redis.As<ChainResponse>().Store(chain, TimeSpan.FromMinutes(2));
                        transactions.ForEach(x => redis.As<TransactionResponse>().Store(x, TimeSpan.FromMinutes(2)));
                    }
                });
            }
            catch
            {
            }
        }
    }
}