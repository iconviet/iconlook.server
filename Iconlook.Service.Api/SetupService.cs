using Agiper.Server;
using Hangfire;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class SetupService : ServiceBase
    {
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
            }

            public void CreateTables()
            {
            }

            public void PopulateTables()
            {
            }
        }

        [Route("/setup", "GET")]
        public class SetupRequest : IReturn<string>, IGet
        {
        }

        public string Any(SetupRequest request)
        {
            BackgroundJob.Enqueue<SetupJob>(x => x.Run());
            return "SETUP JOB IS RUNNING";
        }
    }
}