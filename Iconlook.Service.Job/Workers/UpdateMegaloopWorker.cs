using System.Threading.Tasks;
using Iconlook.Client.Service;
using Iconviet;
using Iconviet.Server;
using Serilog;

namespace Iconlook.Service.Job.Workers
{
    public class UpdateMegaloopWorker : WorkerBase
    {
        public override async Task StartAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Information("{Work} started", nameof(UpdateMegaloopWorker));
                var service = new IconServiceClient(2);
                var megalook = await service.GetScoreApi("");
                var name = megalook[0].GetScoreName();
                var cu = name;
            }
        }
    }
}