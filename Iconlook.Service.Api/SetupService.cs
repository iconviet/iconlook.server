using System;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Hangfire;
using HtmlAgilityPack;
using Iconlook.Entity;
using Iconlook.Object;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Iconlook.Service.Api
{
    public class SetupService : ServiceBase
    {
        [Route("/setup", "GET")]
        public class SetupRequest : IReturn<string>, IGet
        {
        }

        public string Any(SetupRequest _)
        {
            BackgroundJob.Enqueue<SetupJob>(x => x.Run());
            Log.Information("Setup Job Ran.");
            return "SETUP JOB IS RUNNING";
        }

        public class SetupJob : JobBase
        {
            public Task Run()
            {
                DropTables();
                CreateTables();
                return PopulateTables();
            }

            public void DropTables()
            {
                Db.Instance().DropTable<Prep>();
                Db.Instance().DropTable<PrepState_>();
            }

            public void CreateTables()
            {
                Db.Instance().CreateTable<PrepState_>();
                Db.Instance().CreateTable<Prep>();
            }

            public async Task PopulateTables()
            {
                try
                {
                    typeof(PrepState).ToDictionary(1).ForEach(x => Db.Instance().Insert(new PrepState_ { State = (PrepState) x.Key, Description = x.Value }));
                    var html = await new HtmlWeb().LoadFromWebAsync("http://icon.community/iconsensus/candidates");
                    var query = from t in html.DocumentNode.SelectNodes("//tbody")
                                from r in t.SelectNodes("tr")
                                select r;
                    var preps = query.Select(x => new Prep
                    {
                        Joined = DateTime.UtcNow,
                        State = PrepState.Enabled,
                        LastSeen = DateTime.UtcNow,
                        Position = new Random().Next(1, 66),
                        Score = new Random().Next(-100, 100),
                        Voters = new Random().Next(100, 1000),
                        Votes = new Random().Next(1000000, 10000000),
                        Direction = new Random().NextDouble() >= 0.5,
                        RejectedBlocks = new Random().Next(100, 1000),
                        ProducedBlocks = new Random().Next(100000, 1000000),
                        Address = "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf",
                        Name = x.SelectNodes("td")[2].InnerText.Trim().ToTitleCase(),
                        UptimePercentage = new Random().NextDouble() * (0.1 - -0.1) + -0.1,
                        SupplyPercentage = (double) new Random().Next(1000000, 10000000) / 490000000,
                        Location = x.SelectNodes("td")[3].InnerText.Trim().Split(',').Last().ToLower().ToTitleCase(),
                        IdExternal = x.SelectSingleNode("td/a").GetAttributeValue("href", "0").Split('/').ElementAt(3)
                    }).Distinct().OrderBy(x => x.Position).Reverse();
                    await Db.Instance().InsertAllAsync(preps.ToList());
                }
                catch (Exception exception)
                {
                    Log.Error(exception, exception.Message);
                }
            }
        }
    }
}