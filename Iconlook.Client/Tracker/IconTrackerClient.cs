using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Iconlook.Client.Tracker
{
    public class IconTrackerClient
    {
        private readonly JsonHttpClient _client;

        public IconTrackerClient(double timeout = 30)
        {
            _client = new JsonHttpClient("https://tracker.icon.foundation");
            _client.GetHttpClient().Timeout = TimeSpan.FromSeconds(timeout);
        }

        public async Task<MainResponse> GetMainInfo()
        {
            return await _client.GetAsync<MainResponse>("/v0/main/mainInfo");
        }

        public async Task<PageResponse<List<PRepResponse>>> GetPReps()
        {
            return await _client.GetAsync<PageResponse<List<PRepResponse>>>("/v3/iiss/prep/list?count=200");
        }

        public async Task<DelegateListResponse> GetDelegates(string address, int page = 1, int count = 1)
        {
            return await _client.GetAsync<DelegateListResponse>($"/v3/iiss/delegate/list?page={page}&count={count}&prep={address}");
        }
    }
}