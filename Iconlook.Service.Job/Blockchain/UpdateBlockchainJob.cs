using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Blockchain
{
    public class UpdateBlockchainJob : JobBase
    {
        public async Task Run()
        {
            try
            {
                var client = new IconClient();
                var last_block = await client.GetLastBlock();
                var total_supply = await client.GetTotalSupply();
                var transactions = last_block.GetTransactions().Select(x => new TransactionResponse
                {
                    To = x.GetTo().ToString(),
                    From = x.GetFrom().ToString(),
                    Hash = x.GetTxHash().ToString(),
                    Fee = x.GetFee().HasValue ? (decimal) x.GetFee().Value.DividePow(10, 18) : 0,
                    Amount = x.GetValue().HasValue ? (decimal) x.GetValue().Value.DividePow(10, 18) : 0,
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long) x.GetTimestamp().Value.DividePow(10, 3))
                }).ToList();
                var block = new BlockResponse
                {
                    Producer = "ICONVIET",
                    Transactions = transactions.Count,
                    Fee = transactions.Sum(x => x.Fee),
                    Height = (long) last_block.GetHeight(),
                    Amount = transactions.Sum(x => x.Amount),
                    Hash = last_block.GetBlockHash().ToString(),
                    PrevHash = last_block.GetPrevBlockHash().ToString(),
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long) last_block.GetTimestamp().DividePow(10, 3))
                };

                await Channel.Publish(new BlockProducedSignal
                {
                    Block = block
                });
                await Channel.Publish(new BlockchainUpdatedSignal
                {
                    Blockchain = new BlockchainResponse
                    {
                        BlockHeight = block.Height
                    }
                });
                await Endpoint.Publish(new BlockProducedEvent
                {
                    Height = block.Height,
                    Timestamp = block.Timestamp,
                    Transactions = transactions.Select(x => new Transaction
                    {
                        To = x.To,
                        Fee = x.Fee,
                        From = x.From,
                        Hash = x.Hash,
                        Amount = x.Amount
                    }).ToList()
                });
                await Endpoint.Publish(new BlockchainUpdatedEvent
                {
                    BlockHeight = block.Height,
                    Timestamp = block.Timestamp,
                    TotalTransactions = 71098147 + transactions.Count,
                    TokenSupply = (long) total_supply.DividePow(10, 18)
                });
                Redis.Instance().As<BlockResponse>().Store(block, TimeSpan.FromMinutes(1));
                transactions.ForEach(x => Redis.Instance().As<TransactionResponse>().Store(x, TimeSpan.FromMinutes(1)));
            }
            catch (Exception exception)
            {
                Log.Error(exception, nameof(UpdateBlockchainJob));
            }
        }
    }
}