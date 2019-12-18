using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Hangfire;
using Iconlook.Entity;
using Iconlook.Object;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;

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
                PopulateTables();
                return Task.CompletedTask;
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

            public void PopulateTables()
            {
                typeof(PRepState).ToDictionary(1).ForEach(x => Db.Instance().Insert(new PRepState_ { State = (PRepState) x.Key, Description = x.Value }));
            }
        }
    }
}