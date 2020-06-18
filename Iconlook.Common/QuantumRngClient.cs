using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Iconlook.Common
{
    public class QuantumRngClient
    {
        private readonly HttpClient _client;

        public QuantumRngClient()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://random.openqu.org/api/")
            };
        }

        public async Task<int[]> GetIntegers(int min, int max, int size)
        {
            var stream = await _client.GetStreamAsync($"randint?size={size}&min={min}&max={max}");
            using (var document = await JsonDocument.ParseAsync(stream))
            {
                return document.RootElement.GetProperty("result").EnumerateArray().Select(x => x.GetInt32()).ToArray();
            }
        }
    }
}