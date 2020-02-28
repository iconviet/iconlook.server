using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Client.Chainalytic;
using Iconlook.Object;
using Serilog;

namespace Iconlook.Service.Job
{
    public class UpdateAddressJob : JobBase
    {
        public override async Task RunAsync()
        {
            using (var time = new Rolex())
            using (var redis = Redis.Instance())
            {
                redis.As<AddressResponse>().DeleteAll();
                Log.Information("{Job} started", nameof(UpdateAddressJob));
                try
                {
                    var chainalytic = new ChainalyticClient();
                    var unstaking_info = await chainalytic.GetUnstakingInfo();
                    var prep_dictionary = redis.As<PRepResponse>().GetAll().ToDictionary(x => x.Address);
                    redis.As<AddressResponse>().StoreAll(unstaking_info.GetWallets()
                        .Where(x => x.Value.Split(':').Length == 4 && long.TryParse(x.Value.Split(':')[2], out _))
                        .Select(x =>
                        {
                            var tuple = x.Value.Split(':');
                            var name = prep_dictionary.TryGet(x.Key)?.Name;
                            return new AddressResponse
                            {
                                Id = x.Key,
                                Name = name,
                                Hash = x.Key,
                                Type = AddressType.Wallet,
                                UnstakedBlockHeight = long.Parse(tuple[3]),
                                RequestedBlockHeight = long.Parse(tuple[2]),
                                Staked = decimal.Parse(BigDecimal.Parse(tuple[0]).ToString()),
                                Class = name == null ? AddressClass.Iconist : AddressClass.PRep,
                                Unstaking = decimal.Parse(BigDecimal.Parse(tuple[1]).ToString())
                            };
                        }));
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error("{Job} failed to run. {Message}.", nameof(UpdateAddressJob), exception.Message);
                    }
                }
                Log.Information("{Job} stopped ({Elapsed:N0}ms)", nameof(UpdateAddressJob), time.Elapsed.TotalMilliseconds);
            }
        }
    }
}