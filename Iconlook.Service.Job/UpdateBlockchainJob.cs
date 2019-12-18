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
                var transactions = last_block.GetTransactions().Select(x => new TransactionResponse
                {
                    Id = x.GetTxHash().ToString(),
                    Hash = x.GetTxHash().ToString(),
                    Block = (long) last_block.GetHeight(),
                    Fee = x.GetFee().HasValue ? x.GetFee().Value.ToIcxFromLoop() : 0,
                    Amount = x.GetValue().HasValue ? x.GetValue().Value.ToIcxFromLoop() : 0,
                    To = x.GetTo()?.ToString() ?? "cx0000000000000000000000000000000000000000",
                    From = x.GetFrom()?.ToString() ?? "cx0000000000000000000000000000000000000000",
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(x.GetTimestamp().Value.ToMilliseconds())
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
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(last_block.GetTimestamp().ToMilliseconds())
                };
                var chain = new ChainResponse
                {
                    MarketCap = (long) main_info.GetMarketCap(),
                    IcxSupply = (long) main_info.GetIcxSupply(),
                    BlockHeight = (long) iiss_info.GetBlockHeight(),
                    IcxCirculation = (long) main_info.GetIcxCirculation(),
                    PublicTreasury = (long) main_info.GetPublicTreasury(),
                    TransactionCount = (long) main_info.GetTransactionCount(),
                    TotalStaked = (long) prep_info.GetTotalStaked().ToIcxFromLoop(),
                    TotalDelegated = (long) prep_info.GetTotalDelegated().ToIcxFromLoop(),
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(last_block.GetTimestamp().ToMilliseconds())
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
                    Redis.Instance().As<BlockResponse>().Store(block, TimeSpan.FromMinutes(2));
                    Redis.Instance().As<ChainResponse>().Store(chain, TimeSpan.FromMinutes(2));
                    transactions.ForEach(x => Redis.Instance().As<TransactionResponse>().Store(x, TimeSpan.FromMinutes(2)));
                });
            }
            catch
            {
            }
        }
    }
}