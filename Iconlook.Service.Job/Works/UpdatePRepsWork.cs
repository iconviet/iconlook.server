using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Calculator;
using Iconlook.Client;
using Iconlook.Client.Service;
using Iconlook.Client.Tracker;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Lykke.Icon.Sdk.Data;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;
using JsonHttpClient = Iconlook.Client.JsonHttpClient;
using StringExtensions = Agiper.StringExtensions;

namespace Iconlook.Service.Job.Works
{
    public class UpdatePRepsWork : WorkBase
    {
        public override async Task StartAsync()
        {
            using (var time = new Rolex())
            using (var db = Db.Instance())
            using (var redis = Redis.Instance())
            {
                Log.Information("{Work} started", nameof(UpdatePRepsWork));
                try
                {
                    var prep_list = new List<PRep>();
                    var http = new JsonHttpClient(30);
                    var tracker = new IconTrackerClient();
                    var service = new IconServiceClient();
                    var prep_rpcs = await service.GetPReps();
                    var iiss_info = await service.GetIissInfo();
                    var prep_info = await service.GetPRepInfo();
                    var prep_history_list = new List<PRepHistory>();
                    await Task.WhenAll(prep_rpcs.Select(prep => Task.Run(async () =>
                    {
                        try
                        {
                            string logo_url = null;
                            var ranking = prep_rpcs.IndexOf(prep) + 1;
                            prep = await service.GetPRep((Address) prep.GetAddress());
                            var details = prep.GetDetails();
                            if (StringExtensions.HasValue(details))
                            {
                                var response = await http.GetAsync<string>(details);
                                if (response.HasValue() && response.StartsWith('{'))
                                {
                                    var @object = DynamicJson.Deserialize(response);
                                    logo_url = @object?.representative?.logo?.logo_256;
                                }
                                else
                                {
                                    Log.Warning<string>("{Name} : Failed to load details.", prep.GetName());
                                }
                            }
                            var delegates = await tracker.GetDelegates(prep.GetAddress().ToString());
                            prep_list.Add(new PRep
                            {
                                Ranking = ranking,
                                LogoUrl = logo_url,
                                Name = prep.GetName(),
                                City = prep.GetCity(),
                                Joined = DateTime.UtcNow,
                                State = PRepState.Enabled,
                                LastSeen = DateTime.UtcNow,
                                Country = prep.GetCountry(),
                                Voters = delegates.TotalSize,
                                Size = new Random().Next(1, 10),
                                Id = prep.GetAddress().ToString(),
                                P2PEndpoint = prep.GetP2PEndpoint(),
                                Score = new Random().Next(-100, 100),
                                Direction = new Random().NextDouble() >= 0.5,
                                Balance = new Random().Next(100000, 10000000),
                                ProducedBlocks = (long) prep.GetTotalBlocks(),
                                Votes = (long) BigIntegerExtensions.ToIcxFromLoop(prep.GetDelegated()),
                                Testnet = new[] { true, false }[new Random().Next(0, 1)],
                                MissedBlocks = (long) (prep.GetTotalBlocks() - prep.GetValidatedBlocks()),
                                Entity = new[] { "Company", "Group", "Individual" }[new Random().Next(0, 3)],
                                Identity = new[] { "Verified", "Unknown", "Anonymous" }[new Random().Next(0, 3)],
                                Regions = new[] { "Asia", "Europe", "US", "Australia" }[new Random().Next(0, 4)],
                                Goals = new[] { "Development", "Awareness", "Expansion" }[new Random().Next(0, 3)],
                                Hosting = new[] { "Azure", "Amazon", "Google", "Bare Metal" }[new Random().Next(0, 4)],
                                DelegatedPercentage = (double) (BigIntegerExtensions.ToDecimal(prep.GetDelegated()) / prep_info.GetTotalDelegated().ToDecimal()),
                                ProductivityPercentage = prep.GetValidatedBlocks() > 0 ? (double) (BigIntegerExtensions.ToDecimal(prep.GetValidatedBlocks()) / BigIntegerExtensions.ToDecimal(prep.GetTotalBlocks())) : 0
                            }.ThenDo(x =>
                            {
                                prep_history_list.Add(new PRepHistory
                                {
                                    Address = x.Id,
                                    Votes = x.Votes
                                });
                            }));
                        }
                        catch
                        {
                            Log.Warning<string>("{Name} : Failed to load info.", prep.GetName());
                        }
                    })));
                    await db.SaveAllAsync(prep_list.ToList());
                    await db.InsertAllAsync(prep_history_list.ToList());
                    var prep_history_24_h_list = await db.SelectAsync<PRepHistory>(@"
                        SELECT *
                        FROM [PRepHistory] p1
                        WHERE [Timestamp] =
                        (
	                        SELECT MAX([p2].[Timestamp])
	                        FROM [PRepHistory] p2
	                        WHERE [p1].[Address] = [p2].[Address] AND
                                  [p2].[Timestamp] < DATEADD(HOUR, -24, GETUTCDATE())
                        )");
                    redis.StoreAll(prep_list.ConvertAll(e => e.ToResponse().ThenDo(r =>
                    {
                        var irep = iiss_info.GetIRep().ToIcxFromLoop();
                        var p = prep_history_24_h_list.SingleOrDefault(x => r.Id == x.Address);
                        if (p != null)
                        {
                            r.Votes24HChange = e.Votes - p.Votes;
                        }
                        var calculator = new PRepRewardCalculator(irep, r.Ranking, r.DelegatedPercentage);
                        r.DailyReward = (long) calculator.GetDailyReward();
                        r.YearlyReward = (long) calculator.GetYearlyReward();
                        r.MonthlyReward = (long) calculator.GetMonthlyReward();
                        r.MonthlyRewardUsd = r.MonthlyReward * UpdateChainWork.LastIcxPrice;
                    })));
                    Log.Information("**************************************************");
                    Log.Information("{PReps} P-Reps latest information stored in {Elapsed:N0}ms", prep_list.Count, time.Elapsed.TotalMilliseconds);
                    Log.Information("**************************************************");
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error("{Work} failed to run. {Message}.", nameof(UpdatePRepsWork), exception.Message);
                    }
                }
                Log.Information("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdatePeersWork), time.Elapsed.TotalMilliseconds);
            }
        }
    }
}