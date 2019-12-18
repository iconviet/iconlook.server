using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Object;
using Iconlook.Server;
using ServiceStack;

namespace Iconlook.Service.Job
{
    public class UpdatePeersJob : JobBase
    {
        public override async Task RunAsync()
        {
            var redis = Redis.Instance().As<PRepResponse>();
            var preps = redis.GetAll().ToDictionary(x => x.Id);
            if (preps.Any())
            {
                var peers = new List<PeerResponse>();
                await Task.WhenAll(preps.Values.Select(prep =>
                {
                    return Task.Run(async () =>
                    {
                        using (var json = new JsonHttpClient())
                        {
                            var cancelation = new CancellationTokenSource();
                            cancelation.CancelAfter(1500);
                            var endpoint = prep.P2PEndpoint.Replace("7100", "9000");
                            var url = $"http://{endpoint}/api/v1/status/peer";
                            try
                            {
                                var response = await json.GetAsync<string>(url, cancelation.Token);
                                if (response.HasValue())
                                {
                                    var @object = DynamicJson.Deserialize(response);
                                    peers.Add(new PeerResponse
                                    {
                                        Name = prep.Name,
                                        Id = @object.peer_id,
                                        State = @object.state,
                                        Status = @object.status,
                                        PeerId = @object.peer_id,
                                        PeerType = int.Parse(@object.peer_type),
                                        BlockHeight = long.Parse(@object.block_height),
                                        MadeBlockCount = int.Parse(@object.made_block_count),
                                        LeaderMadeBlockCount = int.Parse(@object.leader_made_block_count)
                                    });
                                }
                            }
                            catch
                            {
                            }
                        }
                    });
                }));
                if (peers.Any())
                {
                    await Channel.Publish(new PeersUpdatedSignal
                    {
                        Busy = peers.FirstOrDefault(x => x.State == "BlockGenerate")
                    });
                }
            }
        }
    }
}