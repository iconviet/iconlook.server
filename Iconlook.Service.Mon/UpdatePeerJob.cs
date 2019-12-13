using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client.Service;

namespace Iconlook.Service.Mon
{
    public class UpdatePeerJob : JobBase
    {
        private static readonly IconServiceClient Service;
        private static readonly Dictionary<string, PRep> PReps;
        private static readonly ConcurrentDictionary<string, PRep> Peers;

        static UpdatePeerJob()
        {
            Service = new IconServiceClient();
            PReps = new Dictionary<string, PRep>();
            Peers = new ConcurrentDictionary<string, PRep>();
        }

        public override async Task RunAsync()
        {
            if (!PReps.Any())
            {
                foreach (var prep in (await Service.GetPRepInfo()).GetPReps())
                {
                    PReps.TryAdd(prep.GetAddress().ToString(), prep);
                }
            }
            await Task.WhenAll(PReps.Values.Select(x =>
            {
                return Task.Run(async () =>
                {
                    var prep = await Service.GetPRep(x.GetAddress());
                    Peers.AddOrUpdate(prep.GetP2PEndpoint(), prep, (ip, _) => prep);
                });
            }));
        }
    }
}