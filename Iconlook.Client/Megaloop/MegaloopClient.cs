using System;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Icon.Sdk.Transport.Http;
using Lykke.Icon.Sdk.Transport.JsonRpc;

namespace Iconlook.Client.Megaloop
{
    public class MegaloopClient
    {
        private readonly IconService _client;

        public MegaloopClient(double timeout) : this(Endpoints.MAINNET, timeout)
        {
        }

        public MegaloopClient(string endpoint = Endpoints.MAINNET, double timeout = 30)
        {
            _client = new IconService(new HttpProvider(
                new HttpClient { Timeout = TimeSpan.FromSeconds(timeout) }, $"{endpoint}/api/v3"));
        }

        public async Task<RpcObject> GetPlayers()
        {
            var response = await _client.CallAsync(new Call.Builder()
                .Method("ls_players")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToObject();
        }

        public async Task<string> GetLastPlayer()
        {
            var response = await _client.CallAsync(new Call.Builder()
                .Method("get_last_player")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToString();
        }

        public async Task<string> GetLastWinner()
        {
            var response = await _client.CallAsync(new Call.Builder()
                .Method("get_last_winner")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToString();
        }

        public async Task<BigInteger> GetJackpotSize()
        {
            var response = await _client.CallAsync(new Call.Builder()
                .Method("get_jackpot_size")
                .To(new Address("cxa6ba8f0730ad952b5898ac3e5e90a17e20574eff"))
                .Build());
            return response.ToInteger();
        }
    }
}