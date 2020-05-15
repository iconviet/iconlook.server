using System;
using System.Threading.Tasks;
using Iconlook.Calculator;
using Iconlook.Client;
using Iconlook.Client.Binance;
using Iconlook.Client.Chainalytic;
using Iconlook.Client.Service;
using Iconlook.Client.Tracker;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using Iconviet;
using Iconviet.Server;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Workers
{
    public class UpdateChainWorker : WorkerBase
    {
        public static long LastBlockHeight;
        public static decimal LastIcxPrice;

        public override async Task StartAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Debug("{Work} started", nameof(UpdateChainWorker));
                try
                {
                    var service = new IconServiceClient(2);
                    var last_block = await service.GetLastBlock();
                    if (last_block != null)
                    {
                        if (last_block.GetHeight() > LastBlockHeight)
                        {
                            var binance = new BinanceClient(2);
                            var tracker = new IconTrackerClient(2);
                            var chainalytic = new ChainalyticClient(2);
                            var main_info = await tracker.GetMainInfo();
                            var iiss_info = await service.GetIissInfo();
                            var prep_info = await service.GetPRepInfo();
                            var ticker = await binance.GetTicker("ICXUSDT");
                            var total_supply = await service.GetTotalSupply();
                            var staking_info = await chainalytic.GetStakingInfo();
                            var chain = new ChainResponse
                            {
                                IcxSupply = (long) total_supply.ToIcx(),
                                IRep = iiss_info?.GetIRep().ToIcx() ?? 0,
                                MarketCap = (long) main_info?.GetMarketCap(),
                                IcxCirculation = (long) main_info?.GetIcxCirculation(),
                                PublicTreasury = (long) main_info?.GetPublicTreasury(),
                                Timestamp = last_block.GetTimestamp().ToDateTimeOffset(),
                                TotalStaked = (long) prep_info?.GetTotalStaked().ToIcx(),
                                NextTermBlockHeight = (long) iiss_info?.GetNextPRepTerm(),
                                TransactionCount = (long) main_info?.GetTransactionCount(),
                                IcxPrice = LastIcxPrice = ticker?.LastPrice ?? LastIcxPrice,
                                RRepPercentage = (double) (iiss_info?.GetRRep() * 3) / 10000,
                                IcxPriceChangePercentage = ticker?.PriceChangePercent / 100 ?? 0,
                                BlockHeight = LastBlockHeight = (long) iiss_info?.GetBlockHeight(),
                                StakingAddressCount = (long) staking_info?.GetStakingAddressCount(),
                                TotalDelegated = (long) prep_info?.GetTotalDelegated().ToIcx(),
                                UnstakingAddressCount = (long) staking_info?.GetUnstakingAddressCount(),
                                TotalUnstaking = (long) staking_info?.GetTotalUnstaking().ToBigInteger()
                            };
                            chain.StakedPercentage = (double) chain.TotalStaked / chain.IcxCirculation;
                            chain.DelegatedPercentage = (double) chain.TotalDelegated / chain.IcxSupply;
                            var next_term_calculator = new NextTermCalculator(chain.BlockHeight, chain.NextTermBlockHeight);
                            chain.NextTermLocalTime = next_term_calculator.GetLocalTime();
                            chain.NextTermCountdown = next_term_calculator.GetCountdown();
                            await Channel.Instance().Publish(new ChainUpdatedSignal { Chain = chain }).ConfigureAwait(false);
                            await Endpoint.Instance().Publish(new ChainUpdatedEvent { Chain = chain }).ConfigureAwait(false);
                            await Task.Run(() =>
                            {
                                using (var redis = Redis.Instance())
                                {
                                    redis.As<ChainResponse>().Store(chain, TimeSpan.FromSeconds(60));
                                }
                            }).ConfigureAwait(false);
                        }
                    }
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error(exception, "{Work} failed to run. {Message}", nameof(UpdateChainWorker), exception.Message);
                    }
                }
                Log.Debug("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdateChainWorker), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}