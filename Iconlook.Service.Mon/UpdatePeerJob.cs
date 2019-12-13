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
    public class UpdatePeerJob : JobBase
    {
        private static readonly JsonHttpClient Json;
        private static readonly IconServiceClient Icon;
        private static readonly Dictionary<string, PRep> PReps;
        private static readonly ConcurrentDictionary<string, PRep> Peers;

        static UpdatePeerJob()
        {
            Json = new JsonHttpClient();
            Icon = new IconServiceClient();
            PReps = new Dictionary<string, PRep>();
            Peers = new ConcurrentDictionary<string, PRep>();
            // Json.GetHttpClient().Timeout = TimeSpan.FromSeconds(2);
        }

        public override async Task RunAsync()
        {
            if (!PReps.Any())
            {
                foreach (var prep in (await Icon.GetPRepInfo()).GetPReps())
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
                        peers.Add(new PeerResponse { Address = response.peer_id, State = response.state });
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