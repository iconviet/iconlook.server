using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Message;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Transport.Http;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Blockchain
{
    public class UpdateBlockchainJob : JobBase
    {
        private static readonly HttpClient Http = new HttpClient();

        public async Task Run()
        {
            Log.Information("UpdateBlockchainJob ran");
            var client = new IconService(new HttpProvider(Http, "https://ctz.solidwallet.io/api/v3"));
            var last_block = await client.GetLastBlock();
            var total_supply = await client.GetTotalSupply();
            await Endpoint.Publish(new BlockchainUpdatedEvent
            {
                BlockHeight = (long) last_block.GetHeight(),
                TokenSupply = (long) BigInteger.Divide(total_supply, BigInteger.Pow(10, 18)),
                TotalTransactions = (long) BigInteger.Divide(total_supply, BigInteger.Pow(10, 18))
            });
        }
    }
}