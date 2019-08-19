using System;
using System.Linq;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class BlockService : ServiceBase
    {
        [CacheResponse(Duration = 60, MaxAge = 30)]
        public object Any(BlockListRequest request)
        {
            return new ListResponse<BlockResponse>(Enumerable.Range(1, 20).Select(x => new BlockResponse
            {
                Timestamp = DateTimeOffset.UtcNow,
                Fee = Enumerable.Range(1, 5).Shuffle().Single(),
                Size = Enumerable.Range(1000, 1050).Shuffle().Single(),
                Transactions = Enumerable.Range(1, 5).Shuffle().Single(),
                Height = Enumerable.Range(6000000, 6000100).Shuffle().Single(),
                Amount = new decimal[] { 100, 200, 300, 400, 500 }[new Random().Next(5)],
                Hash = new[]
                {
                    "0xdf1e9ad04468b63047704beb80820ed394de6102ac165bd9ed95eafffa5892ab",
                    "0x528f6fc43ba692b99aeef0a7bd93bc7cd245bce9b90018a02451cfa17d58c33c",
                    "0x12f0821c57bcdaa230706c3cf5b6fd4ff5e8b452b087edd39ab22ed9416caa72",
                    "0xdd6d8abb19f9e7a38198585ab7c5431867dfd1c436940771afd143560f256344",
                    "0x81ca226c940605afe7e3165b4d95f8bf361ec4385e53e9cf2cfe391c61531917"
                }[new Random().Next(5)]
            }).Take(request.Take).ToList());
        }
    }
}