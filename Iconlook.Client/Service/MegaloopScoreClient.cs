using System;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Icon.Sdk.Transport.Http;
using Lykke.Icon.Sdk.Transport.JsonRpc;

namespace Iconlook.Client.Service
{
    public class MegaloopScoreClient : IconServiceClient
    {
        public MegaloopScoreClient(double timeout) : this(Endpoints.TESTNET, timeout)
        {
        }

        public MegaloopScoreClient(string endpoint = Endpoints.TESTNET, double timeout = 30)
        {
            Client = new IconService(new HttpProvider(
                new HttpClient { Timeout = TimeSpan.FromSeconds(timeout) }, $"{endpoint}/api/v3"));
        }

        public async Task<RpcArray> GetPlayers()
        {
            var response = await Client.CallAsync(new Call.Builder()
                .Method("ls_players")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToArray();
        }

        public async Task<RpcArray> GetWinners()
        {
            var response = await Client.CallAsync(new Call.Builder()
                .Method("ls_winners")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToArray();
        }

        public async Task<string> GetLastPlayer()
        {
            var response = await Client.CallAsync(new Call.Builder()
                .Method("get_last_player")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToString();
        }

        public async Task<string> GetLastWinner()
        {
            var response = await Client.CallAsync(new Call.Builder()
                .Method("get_last_winner")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToString();
        }

        public async Task<string> GetCurrentSubsidy()
        {
            var response = await Client.CallAsync(new Call.Builder()
                .Method("get_current_subsidy")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToString();
        }

        public async Task<BigInteger> GetJackpotSize()
        {
            var response = await Client.CallAsync(new Call.Builder()
                .Method("get_jackpot_size")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return BigInteger.Parse(response.ToString());
        }
    }
}