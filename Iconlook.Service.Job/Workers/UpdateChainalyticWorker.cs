using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Iconlook.Calculator;
using Iconlook.Client.Chainalytic;
using Iconlook.Object;
using Iconviet;
using Iconviet.Server;
using Serilog;

namespace Iconlook.Service.Job.Workers
{
    public class UpdateChainalyticWorker : WorkerBase
    {
        public override async Task StartAsync()
        {
            using (var time = new Rolex())
            using (var redis = Redis.Instance())
            {
                Log.Debug("{Work} started", nameof(UpdateChainalyticWorker));
                try
                {
                    var chainalytic = new ChainalyticClient();
                    var unstaking_info = await chainalytic.GetUnstakingInfo();
                    var undelegated_info = await chainalytic.GetUndelegatedInfo();
                    var prep_dictionary = redis.As<PRepResponse>().GetAll().ToDictionary(x => x.Address);
                    redis.As<UnstakingAddressResponse>().DeleteAll();
                    redis.As<UndelegatedAddressResponse>().DeleteAll();
                    redis.As<UndelegatedAddressResponse>().StoreAll(undelegated_info.GetWallets()
                        .Where(x => x.Value.Split(':').Length == 3)
                        .Select(x =>
                        {
                            var (key, value) = x;
                            var tuple = value.Split(':');
                            var name = prep_dictionary.TryGet(key)?.Name;
                            var address = new UndelegatedAddressResponse
                            {
                                Id = key,
                                Hash = key,
                                Name = name,
                                Type = name == null ? AddressType.Iconist : AddressType.PRep,
                                Staked = decimal.Parse(BigDecimal.Parse(tuple[0]).ToString()),
                                Delegated = decimal.Parse(BigDecimal.Parse(tuple[1]).ToString()),
                                Undelegated = decimal.Parse(BigDecimal.Parse(tuple[2]).ToString())
                            };
                            return address;
                        }));
                    redis.As<UnstakingAddressResponse>().StoreAll(unstaking_info.GetWallets()
                        .Where(x => x.Value.Split(':').Length == 4)
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
                                RequestedBlockHeight = long.Parse(tuple[2]),
                                UnstakedBlockHeight = long.Parse(tuple[3]) - 39, // TODO: offset for deviation
                                Type = name == null ? AddressType.Iconist : AddressType.PRep,
                                Staked = decimal.Parse(BigDecimal.Parse(tuple[0]).ToString()),
                                Unstaking = decimal.Parse(BigDecimal.Parse(tuple[1]).ToString())
                            };
                            var calculator = new UnstakeBlockCalculator(
                                UpdateBlockWorker.LastBlockHeight, address.RequestedBlockHeight, address.UnstakedBlockHeight);
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
                        Log.Error(exception, "{Work} failed to run. {Message}. {StackTrace}", nameof(UpdateChainalyticWorker), exception.Message);
                    }
                }
                Log.Debug("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdateChainalyticWorker), time.Elapsed.TotalMilliseconds);
            }
        }
    }
}