using Agiper;
using Agiper.Server;
using Hangfire;
using Iconlook.Entity;
using Iconlook.Object;
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
            return "SETUP JOB IS RUNNING";
        }

        public class SetupJob : JobBase
        {
            public void Run()
            {
                DropTables();
                CreateTables();
                PopulateTables();
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

            public void PopulateTables()
            {
                typeof(PrepState).ToDictionary(1).ForEach(x => Db.Instance().Insert(new PrepState_ { State = (PrepState) x.Key, Description = x.Value }));
            }
        }
    }
}