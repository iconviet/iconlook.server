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

namespace Iconlook.Service.Job.Works
{
    public class UpdateUnstakingWork : WorkBase
    {
        public override async Task StartAsync()
        {
            using (var time = new Rolex())
            using (var redis = Redis.Instance())
            {
                Log.Information("{Work} started", nameof(UpdateUnstakingWork));
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
                                RequestedBlockHeight = long.Parse(tuple[2]),
                                UnstakedBlockHeight = long.Parse(tuple[3]) - 17, // TODO: offset for deviation
                                Staked = decimal.Parse(BigDecimal.Parse(tuple[0]).ToString()),
                                Class = name == null ? AddressClass.Iconist : AddressClass.PRep,
                                Unstaking = decimal.Parse(BigDecimal.Parse(tuple[1]).ToString())
                            };
                            var calculator = new UnstakeBlockCalculator(
                                UpdateBlockWork.LastBlockHeight, address.RequestedBlockHeight, address.UnstakedBlockHeight);
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
                        Log.Error("{Work} failed to run. {Message}.", nameof(UpdateUnstakingWork), exception.Message);
                    }
                }
                Log.Information("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdateUnstakingWork), time.Elapsed.TotalMilliseconds);
            }
        }
    }
}