using Agiper;
using Agiper.Server;
using Hangfire;
using HtmlAgilityPack;
using Iconlook.Entity;
using Iconlook.Object;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Iconlook.Service.Api
{
    public class SetupService : ServiceBase
    {
        [Route("/api/setup", "GET")]
        public class SetupRequest : IReturn<string>, IGet
        {
        }

        public string Any(SetupRequest _)
        {
            BackgroundJob.Enqueue<SetupJob>(x => x.RunAsync());
            Log.Information("Setup Job Ran.");
            return "SETUP JOB IS RUNNING";
        }

        public class SetupJob : JobBase
        {
            public override Task RunAsync()
            {
                DropTables();
                CreateTables();
                return PopulateTables();
            }

            public void DropTables()
            {
                Db.Instance().DropTable<Transaction>();
                Db.Instance().DropTable<Block>();
                Db.Instance().DropTable<PRep>();
                Db.Instance().DropTable<PRepState_>();
            }

            public void CreateTables()
            {
                Db.Instance().CreateTable<PRepState_>();
                Db.Instance().CreateTable<PRep>();
                Db.Instance().CreateTable<Block>();
                Db.Instance().CreateTable<Transaction>();
            }

            public async Task PopulateTables()
            {
                try
                {
                    typeof(PRepState).ToDictionary(1).ForEach(x => Db.Instance().Insert(new PRepState_ { State = (PRepState)x.Key, Description = x.Value }));
                    var html = await new HtmlWeb().LoadFromWebAsync("http://icon.community/iconsensus/candidates");
                    var query = from t in html.DocumentNode.SelectNodes("//tbody")
                                from r in t.SelectNodes("tr")
                                select r;
                    var preps = query.Select(x => new PRep
                    {
                        Joined = DateTime.UtcNow,
                        State = PRepState.Enabled,
                        LastSeen = DateTime.UtcNow,
                        Size = new Random().Next(1, 10),
                        Position = new Random().Next(1, 66),
                        Score = new Random().Next(-100, 100),
                        Voters = new Random().Next(100, 1000),
                        Votes = new Random().Next(1000000, 10000000),
                        Direction = new Random().NextDouble() >= 0.5,
                        Balance = new Random().Next(100000, 10000000),
                        MissedBlocks = new Random().Next(100, 1000),
                        ProducedBlocks = new Random().Next(100000, 1000000),
                        Address = "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf",
                        Testnet = new[] { true, false }[new Random().Next(0, 1)],
                        Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                        ProductivityPercentage = new Random().NextDouble() * (0.1 - -0.1) + -0.1,
                        Entity = new[] { "Company", "Group", "Individual" }[new Random().Next(0, 3)],
                        DelegatedPercentage = (double)new Random().Next(1000000, 10000000) / 490000000,
                        Identity = new[] { "Verified", "Unknown", "Anonymous" }[new Random().Next(0, 3)],
                        Regions = new[] { "Asia", "Europe", "US", "Australia" }[new Random().Next(0, 4)],
                        Goals = new[] { "Development", "Awareness", "Expansion" }[new Random().Next(0, 3)],
                        Hosting = new[] { "Azure", "Amazon", "Google", "Bare Metal" }[new Random().Next(0, 4)],
                        Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').Last().ToLower().ToTitleCase(),
                        IdExternal = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3)
                    });
                    await Db.Instance().InsertAllAsync(preps.Reverse().ToList());
                }
                catch (Exception exception)
                {
                    Log.Error(exception, exception.Message);
                }
            }
        }
    }
}