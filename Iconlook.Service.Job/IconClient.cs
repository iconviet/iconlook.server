using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Icon.Sdk.Transport.Http;
using ServiceStack;

namespace Iconlook.Service.Job
{
    public class IconClient : JsonHttpClient, IIconService
    {
        private readonly IconService _service;

        private static readonly HttpClient IconHttpClient = new HttpClient();

        public IconClient() : base("https://ctz.solidwallet.io/api/v3")
        {
            _service = new IconService(new HttpProvider(HttpClient = IconHttpClient, BaseUri));
        }

        public Task<Block> GetLastBlock()
        {
            return _service.GetLastBlock();
        }

        public Task<Block> GetBlock(Bytes hash)
        {
            return _service.GetBlock(hash);
        }

        public Task<BigInteger> GetTotalSupply()
        {
            return _service.GetTotalSupply();
        }

        public Task<T> CallAsync<T>(Call<T> call)
        {
            return _service.CallAsync(call);
        }

        public Task<Block> GetBlock(BigInteger height)
        {
            return _service.GetBlock(height);
        }

        public Task<BigInteger> GetBalance(Address address)
        {
            return _service.GetBalance(address);
        }

        public Task<List<ScoreApi>> GetScoreApi(Address address)
        {
            return _service.GetScoreApi(address);
        }

        public Task<ConfirmedTransaction> GetTransaction(Bytes hash)
        {
            return _service.GetTransaction(hash);
        }

        public Task<TransactionResult> GetTransactionResult(Bytes hash)
        {
            return _service.GetTransactionResult(hash);
        }

        public Task<Bytes> SendTransaction(SignedTransaction transaction)
        {
            return _service.SendTransaction(transaction);
        }
    }
}