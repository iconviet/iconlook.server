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
        protected IconService Client;

        public IconServiceClient(double timeout) : this(Endpoints.MAINNET, timeout)
        {
        }

        public IconServiceClient(string endpoint = Endpoints.MAINNET, double timeout = 30)
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (environment.HasValue())
            {
                if (environment != Iconviet.Environment.Localhost.ToString())
                {
                    endpoint = Endpoints.CITIZEN;
                }
            }
            Client = new IconService(new HttpProvider(
                new HttpClient { Timeout = TimeSpan.FromSeconds(timeout) }, $"{endpoint}/api/v3"));
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
            return Client.GetLastBlock();
        }

        public Task<Block> GetBlock(Bytes hash)
        {
            return Client.GetBlock(hash);
        }

        public Task<BigInteger> GetTotalSupply()
        {
            return Client.GetTotalSupply();
        }

        public Task<T> CallAsync<T>(Call<T> call)
        {
            return Client.CallAsync(call);
        }

        public Task<Block> GetBlock(BigInteger height)
        {
            return Client.GetBlock(height);
        }

        public Task<BigInteger> GetBalance(Address address)
        {
            return Client.GetBalance(address);
        }

        public Task<List<ScoreApi>> GetScoreApi(Address address)
        {
            return Client.GetScoreApi(address);
        }

        public Task<ConfirmedTransaction> GetTransaction(Bytes hash)
        {
            return Client.GetTransaction(hash);
        }

        public Task<TransactionResult> GetTransactionResult(Bytes hash)
        {
            return Client.GetTransactionResult(hash);
        }

        public Task<Bytes> SendTransaction(SignedTransaction transaction)
        {
            return Client.SendTransaction(transaction);
        }
    }
}