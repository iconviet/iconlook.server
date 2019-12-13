using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceStack;

namespace Iconlook.Client.Tracker
{
    public class IconTrackerClient
    {
        private readonly JsonHttpClient _json;

        private static readonly HttpClient HttpClient = new HttpClient();

        public IconTrackerClient()
        {
            const string url = "https://tracker.icon.foundation";
            _json = new JsonHttpClient(url) { HttpClient = HttpClient };
        }

        public async Task<MainResponse> GetMainInfo()
        {
            var response = await _json.GetAsync<MainResponse>("/v0/main/mainInfo");
            return response;
        }

        public async Task<PageResponse<List<PRepResponse>>> GetPReps()
        {
            var response = await _json.GetAsync<PageResponse<List<PRepResponse>>>("/v3/iiss/prep/list?count=100");
            return response;
        }
    }
}