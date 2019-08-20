using System;
using System.Linq;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class TransactionService : ServiceBase
    {
        public object Any(TransactionListRequest request)
        {
            var response = new ListResponse<TransactionResponse>(Enumerable.Range(1, 20).Select(x => new TransactionResponse
            {
                Timestamp = DateTime.UtcNow,
                To = new[]
                {
                    "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf",
                    "hx15bf6ed47613fc187093e141e55f48be79f8913a",
                    "hxadcb1fa10df99f65a041d3487b94eb51f172d913",
                    "hxfbf161da26bdda5609f8b15773eb1754389eb843",
                    "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf"
                }[new Random().Next(5)],
                From = new[]
                {
                    "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf",
                    "hx15bf6ed47613fc187093e141e55f48be79f8913a",
                    "cxf9148db4f8ec78823a50cb06c4fed83660af38d0",
                    "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf",
                    "hx15bf6ed47613fc187093e141e55f48be79f8913a"
                }[new Random().Next(5)],
                Fee = new float[] { 1, 2, 3, 4, 5 }[new Random().Next(5)],
                BlockHeight = Enumerable.Range(6000000, 100).Shuffle().First(),
                Hash = new[]
                {
                    "0xdf1e9ad04468b63047704beb80820ed394de6102ac165bd9ed95eafffa5892ab",
                    "0x528f6fc43ba692b99aeef0a7bd93bc7cd245bce9b90018a02451cfa17d58c33c",
                    "0x12f0821c57bcdaa230706c3cf5b6fd4ff5e8b452b087edd39ab22ed9416caa72",
                    "0xdd6d8abb19f9e7a38198585ab7c5431867dfd1c436940771afd143560f256344",
                    "0x81ca226c940605afe7e3165b4d95f8bf361ec4385e53e9cf2cfe391c61531917"
                }[new Random().Next(5)],
                Amount = new float[] { 100, 200, 300, 400, 500 }[new Random().Next(5)] * 100000
            }).Take(request.Take).ToList());
            return response;
        }
    }
}