using System;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;

namespace Iconlook.Client.Chainalytic
{
    public class ChainalyticClient
    {
        private readonly JsonHttpClient _client;

        private static readonly HttpClient HttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(2)
        };

        public ChainalyticClient()
        {
            _client = new JsonHttpClient("http://45.76.184.255:5530")
            {
                HttpClient = HttpClient,
                ResultsFilterResponse = (res, dto, method, uri, req) =>
                {
                    if (dto is RpcResponse response)
                    {
                        response.Result = DynamicJson.Deserialize(
                            JsonSerializer.SerializeToString(response.Result));
                    }
                }
            };
        }

        public async Task<StakingInfoRpc> GetStakingInfo()
        {
            using (JsConfig.With(new Config { TextCase = TextCase.SnakeCase }))
            {
                var response = await _client.PostAsync<StakingInfoRpc>("/", new RpcRequest
                {
                    Id = 123,
                    Jsonrpc = "2.0",
                    Method = "_call",
                    Params = new
                    {
                        CallId = "api_call",
                        ApiParams = new { },
                        ApiId = "get_staking_info_last_block"
                    }
                });
                return response ?? new StakingInfoRpc { Id = 123 };
            }
        }
    }
}