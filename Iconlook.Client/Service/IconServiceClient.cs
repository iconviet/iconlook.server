using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Iconviet;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Icon.Sdk.Transport.Http;
using Environment = System.Environment;

namespace Iconlook.Client.Service
{
    public class IconServiceClient : IIconService
    {
        private readonly IconService _client;

        public IconServiceClient(double timeout = 30, string endpoint = "https://ctz.solidwallet.io")
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (environment.HasValue())
            {
                endpoint = environment != "Localhost"
                    ? "http://172.18.0.1:9000" // local
                    : "http://103.92.30.173:9000"; // iconviet-vnnode
            }
            var http_client = new HttpClient { Timeout = TimeSpan.FromSeconds(timeout) };
            _client = new IconService(new HttpProvider(http_client, $"{endpoint}/api/v3"));
        }

        public Task<PRepRpc> GetPRep(string address)
        {
            return GetPRep(new Address(address));
        }

        public Task<BigInteger> GetBalance(string address)
        {
            return GetBalance(new Address(address));
        }

        public Task<List<ScoreApi>> GetScoreApi(string address)
        {
            return GetScoreApi(new Address(address));
        }

        public async Task<IissInfoRpc> GetIissInfo()
        {
            var response = await CallAsync(new Call.Builder()
                .Method("getIISSInfo")
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Build());
            return new IissInfoRpc(response.ToObject());
        }

        public async Task<PRepInfoRpc> GetPRepInfo()
        {
            var response = await CallAsync(new Call.Builder()
                .Method("getPReps")
                .Params(new { endRanking = "1" })
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Build());
            return new PRepInfoRpc(response.ToObject());
        }

        public async Task<PRepRpc> GetPRep(Address address)
        {
            var response = await CallAsync(new Call.Builder()
                .Method("getPRep")
                .Params(new { address })
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Build());
            return new PRepRpc(response.ToObject());
        }

        public async Task<List<PRepRpc>> GetPReps()
        {
            var response = await CallAsync(new Call.Builder()
                .Method("getPReps")
                .Params(new { endRanking = "200" })
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Build());
            return response.ToObject().GetItem("preps").ToArray().Select(x => new PRepRpc(x.ToObject())).ToList();
        }

        public Task<Block> GetLastBlock()
        {
            return _client.GetLastBlock();
        }

        public Task<Block> GetBlock(Bytes hash)
        {
            return _client.GetBlock(hash);
        }

        public Task<BigInteger> GetTotalSupply()
        {
            return _client.GetTotalSupply();
        }

        public Task<T> CallAsync<T>(Call<T> call)
        {
            return _client.CallAsync(call);
        }

        public Task<Block> GetBlock(BigInteger height)
        {
            return _client.GetBlock(height);
        }

        public Task<BigInteger> GetBalance(Address address)
        {
            return _client.GetBalance(address);
        }

        public Task<List<ScoreApi>> GetScoreApi(Address address)
        {
            return _client.GetScoreApi(address);
        }

        public Task<ConfirmedTransaction> GetTransaction(Bytes hash)
        {
            return _client.GetTransaction(hash);
        }

        public Task<TransactionResult> GetTransactionResult(Bytes hash)
        {
            return _client.GetTransactionResult(hash);
        }

        public Task<Bytes> SendTransaction(SignedTransaction transaction)
        {
            return _client.SendTransaction(transaction);
        }
    }
}