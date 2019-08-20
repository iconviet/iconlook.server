using System;
using System.Linq;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class BlockService : ServiceBase
    {
        public object Any(BlockListRequest request)
        {
            var response = new ListResponse<BlockResponse>(Enumerable.Range(1, 20).Select(x => new BlockResponse
            {
                Timestamp = DateTime.UtcNow,
                Fee = Enumerable.Range(1, 5).Shuffle().First(),
                Size = Enumerable.Range(1000, 100).Shuffle().First(),
                Transactions = Enumerable.Range(100, 5).Shuffle().First(),
                Height = Enumerable.Range(6000000, 100).Shuffle().First(),
                ProducerAddress = "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf",
                Hash = new[]
                {
                    "0xdf1e9ad04468b63047704beb80820ed394de6102ac165bd9ed95eafffa5892ab",
                    "0x528f6fc43ba692b99aeef0a7bd93bc7cd245bce9b90018a02451cfa17d58c33c",
                    "0x12f0821c57bcdaa230706c3cf5b6fd4ff5e8b452b087edd39ab22ed9416caa72",
                    "0xdd6d8abb19f9e7a38198585ab7c5431867dfd1c436940771afd143560f256344",
                    "0x81ca226c940605afe7e3165b4d95f8bf361ec4385e53e9cf2cfe391c61531917"
                }[new Random().Next(5)],
                Amount = new[] { 100, 200, 300, 400, 500 }[new Random().Next(5)] * 100000,
                ProduderName = new[] { "01Node", "Pocket/Figment", "ICONVIET", "ICON Foundation", "Everstake" }[new Random().Next(5)]
            }).Take(request.Take).ToList());
            return response;
        }
    }
}