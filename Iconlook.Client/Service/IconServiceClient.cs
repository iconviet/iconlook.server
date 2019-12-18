using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Icon.Sdk.Transport.Http;

namespace Iconlook.Client.Service
{
    public class IconServiceClient : IIconService
    {
        private readonly IconService _icon;

        public IconServiceClient()
        {
            _icon = new IconService(
                new HttpProvider(new HttpClient(),
                    "http://210.180.69.101:9000/api/v3"));
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

        public Task<PRepRpc> GetPRep(string address)
        {
            return GetPRep(new Address(address));
        }

        public Task<Block> GetBlock(BigInteger height)
        {
            return _icon.GetBlock(height);
        }

        public Task<BigInteger> GetBalance(string address)
        {
            return GetBalance(new Address(address));
        }

        public Task<BigInteger> GetBalance(Address address)
        {
            return _icon.GetBalance(address);
        }

        public Task<List<ScoreApi>> GetScoreApi(string address)
        {
            return GetScoreApi(new Address(address));
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
                .Params(new { endRanking = "100" })
                .To(new Address("cx0000000000000000000000000000000000000000"))
                .Build());
            return response.ToObject().GetItem("preps").ToArray().Select(x => new PRepRpc(x.ToObject())).ToList();
        }
    }
}