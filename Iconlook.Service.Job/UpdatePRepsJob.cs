using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Client.Service;
using Iconlook.Entity;
using Iconlook.Message;
using Iconlook.Object;
using ServiceStack.OrmLite;

namespace Iconlook.Service.Job
{
    public class UpdatePRepsJob : JobBase
    {
        private static readonly IconServiceClient Icon = new IconServiceClient();

        public override async Task RunAsync()
        {
            var prep_objs = new List<PRep>();
            var prep_rpcs = await Icon.GetPReps();
            var prep_info = await Icon.GetPRepInfo();
            Redis.Instance().As<PRepResponse>().DeleteAll();
            await Task.WhenAll(prep_rpcs.Select(prep => Task.Run(async () =>
            {
                var ranking = prep_rpcs.IndexOf(prep) + 1;
                prep = await Icon.GetPRep(prep.GetAddress());
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
                });
            })));
            await Db.Instance().SaveAllAsync(prep_objs.ToList());
            Redis.Instance().As<PRepResponse>().StoreAll(prep_objs.ToList().ConvertAll(x => x.ToResponse()));
        }
    }
}