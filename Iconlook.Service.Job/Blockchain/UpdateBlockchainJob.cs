using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Client.Service;
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
        public override async Task RunAsync()
        {
            var client = new IconServiceClient();
            var last_block = await client.GetLastBlock();
            var total_supply = await client.GetTotalSupply();
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
            await Channel.Publish(new BlockchainUpdatedSignal
            {
                Blockchain = new BlockchainResponse
                {
                    BlockHeight = block.Height,
                    TotalTransactions = transactions.Count + 71098147
                }
            });
            await Endpoint.Publish(new BlockchainUpdatedEvent
            {
                BlockHeight = block.Height,
                Timestamp = block.Timestamp,
                TokenSupply = (long) total_supply.ToIcxFromLoop(),
                TotalTransactions = transactions.Count + 71098147
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
    }
}