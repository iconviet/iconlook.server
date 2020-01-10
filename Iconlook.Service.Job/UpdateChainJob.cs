using System;
using System.Linq;
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

        private const string EMPTY_ADDRESS = "cx0000000000000000000000000000000000000000";

        public override async Task RunAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Information("{Job} started", nameof(UpdateChainJob));
                try
                {
                    var service = new IconServiceClient(2);
                    var last_block = await service.GetLastBlock();
                    if (last_block != null)
                    {
                        if (last_block.GetHeight() > LastBlockHeight)
                        {
                            var tracker = new IconTrackerClient(2);
                            var chainalytic = new ChainalyticClient(2);
                            var main_info = await tracker.GetMainInfo();
                            var iiss_info = await service.GetIissInfo();
                            var prep_info = await service.GetPRepInfo();
                            var staking_info = await chainalytic.GetStakingInfo();
                            var transactions = last_block.GetTransactions().Select(x => new TransactionResponse
                            {
                                Id = x.GetTxHash().ToString(),
                                Hash = x.GetTxHash().ToString(),
                                Block = (long) last_block.GetHeight(),
                                To = x.GetTo()?.ToString() ?? EMPTY_ADDRESS,
                                From = x.GetFrom()?.ToString() ?? EMPTY_ADDRESS,
                                Timestamp = x.GetTimestamp().Value.ToDateTimeOffset(),
                                Fee = x.GetFee().HasValue ? x.GetFee().Value.ToIcxFromLoop() : 0,
                                Amount = x.GetValue().HasValue ? x.GetValue().Value.ToIcxFromLoop() : 0
                            }).ToList();
                            var block = new BlockResponse
                            {
                                PeerId = last_block.GetPeerId(),
                                Id = (long) last_block.GetHeight(),
                                Fee = transactions.Sum(x => x.Fee),
                                TransactionCount = transactions.Count,
                                Height = (long) last_block.GetHeight(),
                                Hash = last_block.GetBlockHash().ToString(),
                                TotalAmount = transactions.Sum(x => x.Amount),
                                PrevHash = last_block.GetPrevBlockHash().ToString(),
                                Timestamp = last_block.GetTimestamp().ToDateTimeOffset()
                            };
                            var chain = new ChainResponse
                            {
                                IRep = iiss_info.GetIRep().ToIcxFromLoop(),
                                MarketCap = (long) main_info?.GetMarketCap(),
                                IcxSupply = (long) main_info?.GetIcxSupply(),
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
                            chain.NextTermCountdown = calculator.GetNextTermCountdown();
                            await Task.WhenAll(
                                Channel.Publish(new BlockProducedSignal
                                {
                                    Block = block,
                                    Transactions = transactions
                                }),
                                Endpoint.Publish(new BlockProducedEvent
                                {
                                    Block = block,
                                    Transactions = transactions
                                }),
                                Channel.Publish(new ChainUpdatedSignal { Chain = chain }),
                                Endpoint.Publish(new ChainUpdatedEvent { Chain = chain }),
                                Task.Run(() =>
                                {
                                    using (var redis = Redis.Instance())
                                    {
                                        redis.As<BlockResponse>().Store(block, TimeSpan.FromSeconds(60));
                                        redis.As<ChainResponse>().Store(chain, TimeSpan.FromSeconds(60));
                                        transactions.ForEach(x => redis.As<TransactionResponse>().Store(x, TimeSpan.FromSeconds(60)));
                                    }
                                })
                            );
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Error("{Job} failed to load. {Message}", nameof(UpdateChainJob), exception.Message);
                }
                Log.Information("{Job} stopped. {Elapsed:N0}ms", nameof(UpdateChainJob), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}