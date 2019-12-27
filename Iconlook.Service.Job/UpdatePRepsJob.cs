using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace Iconlook.Service.Job
{
    public class UpdatePRepsJob : JobBase
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        public override async Task RunAsync()
        {
            try
            {
                using (var r1 = new Rolex())
                using (var db = Db.Instance())
                using (var redis = Redis.Instance())
                {
                    var prep_objs = new List<PRep>();
                    var icon = new IconServiceClient();
                    var prep_rpcs = await icon.GetPReps();
                    var prep_info = await icon.GetPRepInfo();
                    var json = new JsonHttpClient { HttpClient = HttpClient };
                    await Task.WhenAll(prep_rpcs.Select(prep => Task.Run(async () =>
                    {
                        using (var r2 = new Rolex())
                        {
                            string logo_url = null;
                            var ranking = prep_rpcs.IndexOf(prep) + 1;
                            prep = await icon.GetPRep(prep.GetAddress());
                            try
                            {
                                var details = prep.GetDetails();
                                if (details.HasValue())
                                {
                                    var response = await json.GetAsync<string>(details);
                                    if (response.HasValue())
                                    {
                                        var @object = DynamicJson.Deserialize(response);
                                        logo_url = @object?.representative?.logo?.logo_256;
                                        // Log.Information("{Name} latest information loaded in {Elapsed}ms", prep.GetName(), r2.Elapsed.TotalMilliseconds);
                                    }
                                }
                            }
                            catch
                            {
                                Log.Warning("{Name} information failed to load in {Elapsed}ms", prep.GetName(), r2.Elapsed.TotalMilliseconds);
                            }
                            prep_objs.Add(new PRep
                            {
                                Ranking = ranking,
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
                                if (logo_url != null) x.LogoUrl = logo_url;
                            }));
                        }
                    })));
                    await db.SaveAllAsync(prep_objs.ToList());
                    redis.StoreAll(prep_objs.ConvertAll(x => x.ToResponse()));
                    Log.Information("**************************************************");
                    Log.Information("{PReps} P-Reps latest information stored in {Elapsed}ms", prep_objs.Count, r1.Elapsed.TotalMilliseconds);
                    Log.Information("**************************************************");
                }
            }
            catch (Exception exception)
            {
                Log.Error("{Job} failed. Error: {Message}", nameof(UpdatePRepsJob), exception.Message);
            }
        }
    }
}