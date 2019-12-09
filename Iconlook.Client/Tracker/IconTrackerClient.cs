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

        public async Task<MainInfo> GetMainInfo()
        {
            var response = await _json.GetAsync<MainInfo>("/v0/main/mainInfo");
            return response;
        }

        public async Task<Response<List<PRep>>> GetPReps()
        {
            var response = await _json.GetAsync<Response<List<PRep>>>("/v3/iiss/prep/list?count=100");
            return response;
        }
    }
}