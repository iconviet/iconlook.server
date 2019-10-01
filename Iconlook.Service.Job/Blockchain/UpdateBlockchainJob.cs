using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Transport.Http;
using NServiceBus;

namespace Iconlook.Service.Job.Blockchain
{
    public class UpdateBlockchainJob : JobBase
    {
        private static readonly HttpClient Http = new HttpClient();

        public async Task Run()
        {
            var icon_service = new IconService(new HttpProvider(Http, "https://ctz.solidwallet.io/api/v3"));
            var last_block = await icon_service.GetLastBlock();
            var total_supply = await icon_service.GetTotalSupply();
            var timestamp = DateTimeOffset.FromUnixTimeMilliseconds((long) last_block.GetTimestamp().ToZeroless(3));
            await Channel.Publish(new BlockProducedSignal
            {
                Block = new BlockResponse
                {
                    Height = 100000
                }
            });
            await Channel.Publish(new BlockchainUpdatedSignal
            {
                Blockchain = new BlockchainResponse
                {
                    BlockHeight = 100000
                }
            });
            await Endpoint.Publish(new BlockchainUpdatedEvent
            {
                Timestamp = timestamp,
                BlockHeight = (long) last_block.GetHeight(),
                TokenSupply = (long) total_supply.ToLooplessIcx(),
                TotalTransactions = 71098147 + last_block.GetTransactions().Count
            });
            await Endpoint.Publish(new BlockProducedEvent
            {
                Timestamp = timestamp,
                Height = (long) last_block.GetHeight(),
                Transactions = last_block.GetTransactions().Select(x => new Transaction
                {
                    To = x.GetTo().ToString(),
                    From = x.GetFrom().ToString(),
                    Hash = x.GetTxHash().ToHexString(true),
                    Amount = x.GetValue() != null ? (decimal) x.GetValue() : 0
                }).ToList()
            });
        }
    }
}