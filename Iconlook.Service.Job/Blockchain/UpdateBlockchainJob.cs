using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using NServiceBus;

namespace Iconlook.Service.Job.Blockchain
{
    public class UpdateBlockchainJob : JobBase
    {
        public async Task Run()
        {
            var client = new IconClient();
            var last_block = await client.GetLastBlock();
            var transactions = last_block.GetTransactions();
            var total_supply = await client.GetTotalSupply();
            var timestamp = (long) last_block.GetTimestamp().DividePow(10, 3);
            var block = new BlockResponse
            {
                Producer = "ICONVIET",
                Transactions = transactions.Count,
                Height = (long) last_block.GetHeight(),
                Hash = last_block.GetBlockHash().ToString(),
                PrevHash = last_block.GetPrevBlockHash().ToString(),
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestamp),
                Fee = transactions.Sum(x => x.GetFee() == null ? 0 : (decimal) x.GetFee().Value.DividePow(10, 18)),
                Amount = transactions.Sum(x => x.GetValue() == null ? 0 : (decimal) x.GetValue().Value.DividePow(10, 18))
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
            await Endpoint.Publish(new BlockchainUpdatedEvent
            {
                BlockHeight = block.Height,
                Timestamp = block.Timestamp,
                TokenSupply = (long) total_supply.DividePow(10, 18),
                TotalTransactions = 71098147 + last_block.GetTransactions().Count
            });
            await Endpoint.Publish(new BlockProducedEvent
            {
                Height = block.Height,
                Timestamp = block.Timestamp,
                Transactions = last_block.GetTransactions().Select(x => new Transaction
                {
                    To = x.GetTo().ToString(),
                    From = x.GetFrom().ToString(),
                    Hash = x.GetTxHash().ToHexString(true),
                    Amount = x.GetValue() != null ? (decimal) x.GetValue() : 0
                }).ToList()
            });
            Redis.Instance().As<BlockResponse>().Store(block, TimeSpan.FromMinutes(1));
        }
    }
}