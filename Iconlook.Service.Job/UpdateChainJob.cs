using System;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Calculator;
using Iconlook.Client;
using Iconlook.Client.Chainalytic;
using Iconlook.Client.Service;
using Iconlook.Client.Tracker;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job
{
    public class UpdateChainJob : JobBase
    {
        public static long LastBlockHeight;

        public override async Task RunAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Information("{Job} started", nameof(UpdateChainJob));
                try
                {
                    var service = new IconServiceClient(2);
                    var last_block = await service.GetLastBlock();
                    var total_supply = await service.GetTotalSupply();
                    if (last_block != null)
                    {
                        if (last_block.GetHeight() > LastBlockHeight)
                        {
                            var tracker = new IconTrackerClient();
                            var chainalytic = new ChainalyticClient(2);
                            var main_info = await tracker.GetMainInfo();
                            var iiss_info = await service.GetIissInfo();
                            var prep_info = await service.GetPRepInfo();
                            var staking_info = await chainalytic.GetStakingInfo();
                            var chain = new ChainResponse
                            {
                                IRep = iiss_info.GetIRep().ToIcxFromLoop(),
                                MarketCap = (long) main_info?.GetMarketCap(),
                                IcxSupply = (long) total_supply.ToIcxFromLoop(),
                                IcxCirculation = (long) main_info?.GetIcxCirculation(),
                                PublicTreasury = (long) main_info?.GetPublicTreasury(),
                                Timestamp = last_block.GetTimestamp().ToDateTimeOffset(),
                                NextTermBlockHeight = (long) iiss_info.GetNextPRepTerm(),
                                TransactionCount = (long) main_info?.GetTransactionCount(),
                                RRepPercentage = (double) (iiss_info.GetRRep() * 3) / 10000,
                                TotalStaked = (long) prep_info?.GetTotalStaked().ToIcxFromLoop(),
                                BlockHeight = LastBlockHeight = (long) iiss_info?.GetBlockHeight(),
                                StakingAddressCount = (long) staking_info?.GetStakingAddressCount(),
                                TotalDelegated = (long) prep_info?.GetTotalDelegated().ToIcxFromLoop(),
                                UnstakingAddressCount = (long) staking_info?.GetUnstakingAddressCount(),
                                TotalUnstaking = (long) staking_info?.GetTotalUnstaking().ToBigInteger()
                            };
                            chain.IcxPrice = (decimal) chain.MarketCap / chain.IcxCirculation;
                            chain.StakedPercentage = (double) chain.TotalStaked / chain.IcxSupply;
                            chain.DelegatedPercentage = (double) chain.TotalDelegated / chain.IcxSupply;
                            var calculator = new BlockCalculator(chain.BlockHeight, chain.NextTermBlockHeight);
                            chain.NextTermLocalTime = calculator.GetNextTermLocalTime();
                            chain.NextTermCountdown = calculator.GetNextTermCountdown();
                            await Channel.Publish(new ChainUpdatedSignal { Chain = chain }).ConfigureAwait(false);
                            await Endpoint.Publish(new ChainUpdatedEvent { Chain = chain }).ConfigureAwait(false);
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
                    Log.Error("{Job} failed to run. {Message}", nameof(UpdateChainJob), exception.Message);
                }
                Log.Information("{Job} stopped ({Elapsed:N0}ms)", nameof(UpdateChainJob), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}