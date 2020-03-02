using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Client.Service;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;
using JsonHttpClient = Iconlook.Client.JsonHttpClient;

namespace Iconlook.Service.Job
{
    public class UpdatePRepsJob : JobBase
    {
        public override async Task RunAsync()
        {
            using (var time = new Rolex())
            using (var db = Db.Instance())
            using (var redis = Redis.Instance())
            {
                Log.Information("{Job} started", nameof(UpdatePRepsJob));
                try
                {
                    var prep_list = new List<PRep>();
                    var json = new JsonHttpClient(60);
                    var icon = new IconServiceClient();
                    var prep_rpcs = await icon.GetPReps();
                    var prep_info = await icon.GetPRepInfo();
                    var prep_history_list = new List<PRepHistory>();
                    await Task.WhenAll(prep_rpcs.Select(prep => Task.Run(async () =>
                    {
                        try
                        {
                            string logo_url = null;
                            var ranking = prep_rpcs.IndexOf(prep) + 1;
                            prep = await icon.GetPRep(prep.GetAddress());
                            var details = prep.GetDetails();
                            if (details.HasValue())
                            {
                                var response = await json.GetAsync<string>(details);
                                if (response.HasValue() && response.StartsWith('{'))
                                {
                                    var @object = DynamicJson.Deserialize(response);
                                    logo_url = @object?.representative?.logo?.logo_256;
                                }
                                else
                                {
                                    Log.Warning("{Name} : Failed to load details.", prep.GetName());
                                }
                            }
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
                                Size = new Random().Next(1, 10),
                                Id = prep.GetAddress().ToString(),
                                P2PEndpoint = prep.GetP2PEndpoint(),
                                Score = new Random().Next(-100, 100),
                                Voters = new Random().Next(100, 1000),
                                Direction = new Random().NextDouble() >= 0.5,
                                Balance = new Random().Next(100000, 10000000),
                                ProducedBlocks = (long) prep.GetTotalBlocks(),
                                Votes = (long) prep.GetDelegated().ToIcxFromLoop(),
                                Testnet = new[] { true, false }[new Random().Next(0, 1)],
                                MissedBlocks = (long) (prep.GetTotalBlocks() - prep.GetValidatedBlocks()),
                                Entity = new[] { "Company", "Group", "Individual" }[new Random().Next(0, 3)],
                                Identity = new[] { "Verified", "Unknown", "Anonymous" }[new Random().Next(0, 3)],
                                Regions = new[] { "Asia", "Europe", "US", "Australia" }[new Random().Next(0, 4)],
                                Goals = new[] { "Development", "Awareness", "Expansion" }[new Random().Next(0, 3)],
                                Hosting = new[] { "Azure", "Amazon", "Google", "Bare Metal" }[new Random().Next(0, 4)],
                                DelegatedPercentage = (double) (prep.GetDelegated().ToDecimal() / prep_info.GetTotalDelegated().ToDecimal()),
                                ProductivityPercentage = prep.GetValidatedBlocks() > 0 ? (double) (prep.GetValidatedBlocks().ToDecimal() / prep.GetTotalBlocks().ToDecimal()) : 0
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
                            Log.Warning("{Name} : Failed to load information.", prep.GetName());
                        }
                    })));
                    await db.SaveAllAsync(prep_list.ToList());
                    await db.SaveAllAsync(prep_history_list.ToList());
                    redis.StoreAll(prep_list.ConvertAll(entity => entity.ToResponse().ThenDo(response =>
                    {
                        // var prep_history = db.Select<PRepHistory>().FirstOrDefault(history => // TODO: optimize this query
                        //     response.Id == history.Address && history.Timestamp < DateTime.UtcNow.AddHours(-24));
                        // if (prep_history != null)
                        // {
                        //     response.Votes24HChange = entity.Votes - prep_history.Votes;
                        // }
                    })));
                    Log.Information("**************************************************");
                    Log.Information("{PReps} P-Reps latest information stored in {Elapsed:N0}ms", prep_list.Count, time.Elapsed.TotalMilliseconds);
                    Log.Information("**************************************************");
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error("{Job} failed to run. {Message}. {StackTrace}.", nameof(UpdatePRepsJob), exception.Message, exception.StackTrace);
                    }
                }
                Log.Information("{Job} stopped ({Elapsed:N0}ms)", nameof(UpdatePeersJob), time.Elapsed.TotalMilliseconds);
            }
        }
    }
}