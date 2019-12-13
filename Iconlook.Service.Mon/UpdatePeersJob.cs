using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client.Service;
using Iconlook.Object;
using Iconlook.Server;
using ServiceStack;

namespace Iconlook.Service.Mon
{
    public class UpdatePeersJob : JobBase
    {
        private static readonly JsonHttpClient Json = new JsonHttpClient();
        private static readonly IconServiceClient Icon = new IconServiceClient();
        private static readonly Dictionary<string, PRep> PReps = new Dictionary<string, PRep>();
        private static readonly ConcurrentDictionary<string, PRep> Peers = new ConcurrentDictionary<string, PRep>();

        public override async Task RunAsync()
        {
            if (!PReps.Any())
            {
                foreach (var prep in await Icon.GetPReps())
                {
                    PReps.TryAdd(prep.GetAddress().ToString(), prep);
                }
            }
            if (!Peers.Any())
            {
                await Task.WhenAll(PReps.Values.Select(item =>
                {
                    return Task.Run(async () =>
                    {
                        var prep = await Icon.GetPRep(item.GetAddress());
                        Peers.AddOrUpdate(prep.GetP2PEndpoint(), prep, (ip, _) => prep);
                    });
                }));
            }
            var peers = new List<PeerResponse>();
            await Task.WhenAll(Peers.Select(item =>
            {
                return Task.Run(async () =>
                {
                    try
                    {
                        var cancelation = new CancellationTokenSource();
                        cancelation.CancelAfter(1500);
                        var endpoint = item.Key.Replace("7100", "9000");
                        var url = $"http://{endpoint}/api/v1/status/peer";
                        var json = await Json.GetAsync<string>(url, cancelation.Token);
                        var response = DynamicJson.Deserialize(json);
                        peers.Add(new PeerResponse
                        {
                            State = response.state,
                            Status = response.status,
                            PeerId = response.peer_id,
                            Name = PReps[response.peer_id].GetName(),
                            PeerType = int.Parse(response.peer_type),
                            BlockHeight = long.Parse(response.block_height)
                        });
                    }
                    catch
                    {
                    }
                });
            }));
            await Channel.Publish(new PeersUpdatedSignal { Peers = peers });
        }
    }
}