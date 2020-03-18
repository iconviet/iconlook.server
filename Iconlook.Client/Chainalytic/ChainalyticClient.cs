using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;

namespace Iconlook.Client.Chainalytic
{
    public class ChainalyticClient
    {
        private readonly JsonHttpClient _client;

        public ChainalyticClient(double timeout = 30)
        {
            _client = new JsonHttpClient("http://140.82.11.203:5530")
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

        public async Task<UnstakingInfoRpc> GetUnstakingInfo()
        {
            using (JsConfig.With(new Config { TextCase = TextCase.SnakeCase }))
            {
                var response = await _client.PostAsync<UnstakingInfoRpc>("/", new RpcRequest
                {
                    Id = 123,
                    Jsonrpc = "2.0",
                    Method = "_call",
                    Params = new
                    {
                        CallId = "api_call",
                        ApiId = "latest_unstake_state",
                        ApiParams = new
                        {
                            transform_id = "stake_history"
                        }
                    }
                });
                return response ?? new UnstakingInfoRpc { Id = 123 };
            }
        }

        public async Task<UndelegatedInfoRpc> GetUndelegatedInfo()
        {
            using (JsConfig.With(new Config { TextCase = TextCase.SnakeCase }))
            {
                var response = await _client.PostAsync<UndelegatedInfoRpc>("/", new RpcRequest
                {
                    Id = 123,
                    Jsonrpc = "2.0",
                    Method = "_call",
                    Params = new
                    {
                        CallId = "api_call",
                        ApiId = "abstention_stake"
                    }
                });
                return response ?? new UndelegatedInfoRpc { Id = 123 };
            }
        }
    }
}