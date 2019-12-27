using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Iconlook.Client.Tracker
{
    public class IconTrackerClient
    {
        private readonly JsonHttpClient _client;

        private static readonly HttpClient HttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        public IconTrackerClient()
        {
            const string url = "https://tracker.icon.foundation";
            _client = new JsonHttpClient(url) { HttpClient = HttpClient };
        }

        public async Task<MainResponse> GetMainInfo()
        {
            var response = await _client.GetAsync<MainResponse>("/v0/main/mainInfo");
            return response;
        }

        public async Task<PageResponse<List<PRepResponse>>> GetPReps()
        {
            var response = await _client.GetAsync<PageResponse<List<PRepResponse>>>("/v3/iiss/prep/list?count=100");
            return response;
        }
    }
}