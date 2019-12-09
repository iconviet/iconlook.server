using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Icon.Sdk.Transport.Http;
using Lykke.Icon.Sdk.Transport.JsonRpc;
using ServiceStack;

namespace Iconlook.Client.Service
{
    public class IconServiceClient : IIconService
    {
        private readonly IconService _icon;
        private readonly JsonHttpClient _json;

        private static readonly HttpClient HttpClient = new HttpClient();

        public IconServiceClient()
        {
            const string url = "http://95.179.230.6:9000/api/v3";
            _icon = new IconService(new HttpProvider(HttpClient, url));
            _json = new JsonHttpClient(url) { HttpClient = HttpClient };
        }

        public Task<Block> GetLastBlock()
        {
            return _icon.GetLastBlock();
        }

        public Task<Block> GetBlock(Bytes hash)
        {
            return _icon.GetBlock(hash);
        }

        public Task<BigInteger> GetTotalSupply()
        {
            return _icon.GetTotalSupply();
        }

        public Task<T> CallAsync<T>(Call<T> call)
        {
            return _icon.CallAsync(call);
        }

        public Task<Block> GetBlock(BigInteger height)
        {
            return _icon.GetBlock(height);
        }

        public Task<BigInteger> GetBalance(Address address)
        {
            return _icon.GetBalance(address);
        }

        public Task<List<ScoreApi>> GetScoreApi(Address address)
        {
            return _icon.GetScoreApi(address);
        }

        public Task<ConfirmedTransaction> GetTransaction(Bytes hash)
        {
            return _icon.GetTransaction(hash);
        }

        public Task<TransactionResult> GetTransactionResult(Bytes hash)
        {
            return _icon.GetTransactionResult(hash);
        }

        public Task<Bytes> SendTransaction(SignedTransaction transaction)
        {
            return _icon.SendTransaction(transaction);
        }

        public async Task<IissInfo> GetIissInfo()
        {
            var response = await CallAsync(new Call.Builder()
                .Method("getIISSInfo")
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Build());
            return new IissInfo(response.ToObject());
        }

        public async Task<PRep> GetPRep(Address address)
        {
            var response = await CallAsync(new Call.Builder()
                .Method("getPRep")
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Params(new RpcObject.Builder().Put("address", new RpcValue(address)).Build())
                .Build());
            return new PRep(response.ToObject());
        }

        public async Task<List<PRep>> GetPReps()
        {
            var response = await CallAsync(new Call.Builder()
                .Method("getPReps")
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Build());
            return response.ToObject().GetItem("preps").ToArray().Select(x => new PRep(x.ToObject())).ToList();
        }
    }
}