using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
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
        public async Task Run()
        {
            var client = new IconClient();
            var last_block = await client.GetLastBlock();
            var total_supply = await client.GetTotalSupply();
            var transactions = last_block.GetTransactions().Select(x => new Transaction
            {
                To = x.GetTo().ToString(),
                From = x.GetFrom().ToString(),
                Hash = x.GetTxHash().ToString(),
                Block = (long) last_block.GetHeight(),
                Fee = x.GetFee().HasValue ? (decimal) x.GetFee().Value.DividePow(10, 18) : 0,
                Amount = x.GetValue().HasValue ? (decimal) x.GetValue().Value.DividePow(10, 18) : 0,
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long) x.GetTimestamp().Value.DividePow(10, 3))
            }).ToList();
            var block = new Block
            {
                PeerId = last_block.GetPeerId(),
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
                Block = block.ToResponse()
            });
            await Endpoint.Publish(new BlockProducedEvent
            {
                Block = block,
                Transactions = transactions
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
                TotalTransactions = 71098147 + transactions.Count,
                TokenSupply = (long) total_supply.DividePow(10, 18)
            });
            await Task.Run(async () =>
            {
                var db = Db.Instance();
                if (!db.Exists<Block>(x => x.Height == block.Height))
                {
                    await db.InsertAsync(block);
                    await db.InsertAllAsync(transactions);
                    var block_redis = Redis.Instance().As<BlockResponse>();
                    block_redis.Store(block.ToResponse(), TimeSpan.FromMinutes(1));
                    var transaction_redis = Redis.Instance().As<TransactionResponse>();
                    transactions.ForEach(x => transaction_redis.Store(x.ToResponse(), TimeSpan.FromMinutes(1)));
                }
            });
        }
    }
}