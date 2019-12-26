using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Object;
using Iconlook.Server;
using Serilog;
using ServiceStack;

namespace Iconlook.Service.Job
{
    public class UpdatePeersJob : JobBase
    {
        public override async Task RunAsync()
        {
            try
            {
                using (var redis = Redis.Instance())
                {
                    var items = redis.As<PRepResponse>().GetAll().ToDictionary(x => x.Id);
                    if (items.Any())
                    {
                        var peers = new List<PeerResponse>();
                        await Task.WhenAll(items.Values.Select(prep =>
                        {
                            return Task.Run(async () =>
                            {
                                using (var json = new JsonHttpClient())
                                {
                                    var cancelation = new CancellationTokenSource();
                                    cancelation.CancelAfter(1000);
                                    var endpoint = prep.P2PEndpoint.Replace("7100", "9000");
                                    var url = $"http://{endpoint}/api/v1/status/peer";
                                    try
                                    {
                                        var response = await json.GetAsync<string>(url, cancelation.Token);
                                        if (response.HasValue())
                                        {
                                            var @object = DynamicJson.Deserialize(response);
                                            peers.Add(prep.ConvertTo<PeerResponse>().ThenDo(x =>
                                            {
                                                x.Id = @object.peer_id;
                                                x.State = @object.state;
                                                x.Status = @object.status;
                                                x.PeerId = @object.peer_id;
                                                x.Name = x.Name.SafeSubstring(0, 24);
                                                x.PeerType = int.Parse(@object.peer_type);
                                                x.BlockHeight = long.Parse(@object.block_height);
                                                x.MadeBlockCount = int.Parse(@object.made_block_count);
                                                x.LeaderMadeBlockCount = int.Parse(@object.leader_made_block_count);
                                            }));
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
                            redis.StoreAll(peers);
                            await Channel.Publish(new PeersUpdatedSignal
                            {
                                Sync = peers.Where(x => x.State == "BlockSync").ToList(),
                                Busy = peers.Where(x => x.State == "BlockGenerate").ToList(),
                                Down = peers.Where(x => x.State == "LeaderComplain").ToList()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("{Job} failed. Error: {Message}", nameof(UpdatePeersJob), ex.Message);
            }
        }
    }
}