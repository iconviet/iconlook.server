using System;
using System.Threading.Tasks;

namespace Iconlook.Client.Binance
{
    public class BinanceApiClient
    {
        private readonly JsonHttpClient _client;

        public BinanceApiClient(double timeout = 30)
        {
            _client = new JsonHttpClient("http://api.binance.com");
            _client.GetHttpClient().Timeout = TimeSpan.FromSeconds(timeout);
        }

        public async Task<TickerResponse> GetTicker(string symbol)
        {
            var response = await _client.GetAsync<TickerResponse>($"/api/v3/ticker/24hr?symbol={symbol}");
            return response ?? new TickerResponse();
        }
    }
}