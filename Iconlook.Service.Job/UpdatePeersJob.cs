using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Object;
using Iconlook.Server;
using Serilog;
using ServiceStack;
using JsonHttpClient = Iconlook.Client.JsonHttpClient;

namespace Iconlook.Service.Job
{
    public class UpdatePeersJob : JobBase
    {
        public override async Task RunAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Information("{Job} started", nameof(UpdatePeersJob));
                try
                {
                    using (var redis = Redis.Instance())
                    {
                        var items = redis.As<PRepResponse>().GetAll().ToDictionary(x => x.Id);
                        if (items.Any())
                        {
                            var peers = new List<PeerResponse>();
                            await Task.WhenAll(items.Values.Where(x => x != null).Select(prep =>
                            {
                                return Task.Run(async () =>
                                {
                                    var client = new JsonHttpClient(2);
                                    var endpoint = prep.P2PEndpoint.Replace("7100", "9000");
                                    var url = $"http://{endpoint}/api/v1/status/peer";
                                    var response = await client.GetAsync<string>(url);
                                    if (response.HasValue())
                                    {
                                        var @object = DynamicJson.Deserialize(response);
                                        peers.Add(prep.ConvertTo<PeerResponse>().ThenDo(x =>
                                        {
                                            x.Name = x.Name;
                                            x.Id = @object.peer_id;
                                            x.State = @object.state;
                                            x.Status = @object.status;
                                            x.PeerId = @object.peer_id;
                                            x.PeerType = int.Parse(@object.peer_type);
                                            x.BlockHeight = long.Parse(@object.block_height);
                                            x.MadeBlockCount = int.Parse(@object.made_block_count);
                                            x.LeaderMadeBlockCount = int.Parse(@object.leader_made_block_count);
                                        }));
                                    }
                                });
                            }));
                            if (peers.Any())
                            {
                                await Channel.Publish(new PeersUpdatedSignal
                                {
                                    Idle = peers.Where(x => x.State == "Vote").ToList(),
                                    Sync = peers.Where(x => x.State == "BlockSync").ToList(),
                                    Busy = peers.Where(x => x.State == "BlockGenerate").ToList(),
                                    Down = peers.Where(x => x.State == "LeaderComplain").ToList()
                                }).ConfigureAwait(false);
                                await Task.Run(() => redis.StoreAll(peers)).ConfigureAwait(false);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Error("{Job} failed to run. {Message}", nameof(UpdatePeersJob), exception.Message);
                }
                Log.Information("{Job} stopped ({Elapsed:N0}ms)", nameof(UpdatePeersJob), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}