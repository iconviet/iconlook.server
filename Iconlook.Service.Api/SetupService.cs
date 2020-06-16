using System.Data;
using System.Threading.Tasks;
using Hangfire;
using Iconlook.Entity;
using Iconlook.Object;
using Iconviet;
using Iconviet.Server;
using Serilog;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Iconlook.Service.Api
{
    public class SetupService : ServiceBase
    {
        [Route("/api/v1/setup", "GET")]
        public class SetupRequest : IReturn<string>, IGet
        {
        }

        public string Any(SetupRequest _)
        {
            BackgroundJob.Enqueue<SetupJob>(x => x.StartAsync());
            Log.Information("Setup Job Ran.");
            return "SETUP JOB IS RUNNING";
        }

        public class SetupJob : JobBase
        {
            private IDbConnection _db;

            public override Task StartAsync()
            {
                using (_db = Db.Instance())
                {
                    DropTables();
                    CreateTables();
                    PopulateTables();
                    return Task.CompletedTask;
                }
            }

            public void DropTables()
            {
                _db.DropTable<Transaction>();
                _db.DropTable<Block>();
                _db.DropTable<PRepHistory>();
                _db.DropTable<PRep>();
                _db.DropTable<PRepState_>();
            }

            public void CreateTables()
            {
                _db.CreateTable<PRepState_>();
                _db.CreateTable<PRep>();
                _db.CreateTable<PRepHistory>();
                _db.CreateTable<Block>();
                _db.CreateTable<Transaction>();
            }

            public void PopulateTables()
            {
                typeof(PRepState).ToDictionary(1).ForEach(x => _db.Insert(new PRepState_ { State = (PRepState) x.Key, Description = x.Value }));
            }
        }
    }
}