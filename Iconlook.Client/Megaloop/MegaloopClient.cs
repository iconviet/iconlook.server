using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;

namespace Iconlook.Client.Megaloop
{
    public class MegaloopClient
    {
        private readonly JsonHttpClient _client;

        public MegaloopClient(double timeout = 30)
        {
            _client = new JsonHttpClient("https://bicon.net.solidwallet.io")
            {
                ResultsFilterResponse = (res, dto, method, uri, req) =>
                {
                    if (dto is RpcResponse response)
                        response.Result = DynamicJson.Deserialize(
                            JsonSerializer.SerializeToString(response.Result));
                }
            };
            _client.GetHttpClient().Timeout = TimeSpan.FromSeconds(timeout);
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