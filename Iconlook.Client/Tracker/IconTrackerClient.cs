using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;

namespace Iconlook.Client.Tracker
{
    public class IconTrackerClient
    {
        private readonly JsonHttpClient _json;

        private static readonly HttpClient HttpClient = new HttpClient();

        public IconTrackerClient()
        {
            const string url = "https://tracker.icon.foundation/v3";
            _json = new JsonHttpClient(url) { HttpClient = HttpClient };
        }

        public async Task<List<PRep>> GetPReps()
        {
            var response = await _json.GetAsync<JsonObject>("/iiss/prep/list?count=100");
            return response.Get<List<PRep>>("data");
        }
    }
}