using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Calculator;
using Iconlook.Client.Chainalytic;
using Iconlook.Object;
using Serilog;

namespace Iconlook.Service.Job
{
    public class UpdateUnstakingJob : JobBase
    {
        public override async Task RunAsync()
        {
            using (var time = new Rolex())
            using (var redis = Redis.Instance())
            {
                Log.Information("{Job} started", nameof(UpdateUnstakingJob));
                try
                {
                    var chainalytic = new ChainalyticClient();
                    var unstaking_info = await chainalytic.GetUnstakingInfo();
                    var prep_dictionary = redis.As<PRepResponse>().GetAll().ToDictionary(x => x.Address);
                    redis.As<UnstakingAddressResponse>().DeleteAll();
                    redis.As<UnstakingAddressResponse>().StoreAll(unstaking_info.GetWallets()
                        .Where(x => x.Value.Split(':').Length == 4 && long.TryParse(x.Value.Split(':')[2], out _))
                        .Select(x =>
                        {
                            var (key, value) = x;
                            var tuple = value.Split(':');
                            var name = prep_dictionary.TryGet(key)?.Name;
                            var address = new UnstakingAddressResponse
                            {
                                Id = key,
                                Hash = key,
                                Name = name,
                                Type = AddressType.Wallet,
                                UnstakedBlockHeight = long.Parse(tuple[3]),
                                RequestedBlockHeight = long.Parse(tuple[2]),
                                Staked = decimal.Parse(BigDecimal.Parse(tuple[0]).ToString()),
                                Class = name == null ? AddressClass.Iconist : AddressClass.PRep,
                                Unstaking = decimal.Parse(BigDecimal.Parse(tuple[1]).ToString())
                            };
                            var calculator = new UnstakeBlockCalculator(
                                UpdateBlockJob.LastBlockHeight, address.RequestedBlockHeight, address.UnstakedBlockHeight);
                            address.RequestedDateTime = calculator.GetRequestDateTime();
                            address.UnstakingCountdown = calculator.GetUnstakingCountdown();
                            address.RequestedDateTimeAge = calculator.GetRequestDateTimeAge();
                            address.UnstakingCountdownShort = calculator.GetUnstakingCountdownShort();
                            address.RequestedDateTimeAgeShort = calculator.GetRequestDateTimeAgeShort();
                            return address;
                        }));
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error("{Job} failed to run. {Message}.", nameof(UpdateUnstakingJob), exception.Message);
                    }
                }
                Log.Information("{Job} stopped ({Elapsed:N0}ms)", nameof(UpdateUnstakingJob), time.Elapsed.TotalMilliseconds);
            }
        }
    }
}