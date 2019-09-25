using System.Net.Http;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Message;
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
            await Endpoint.Publish(new BlockchainUpdatedEvent
            {
                BlockHeight = (long) last_block.GetHeight(),
                TokenSupply = (long) total_supply.ToLooplessIcx(),
                TotalTransactions = (long) total_supply.ToLooplessIcx()
            });
        }
    }
}