using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iconlook.Message;
using Iconviet;
using Iconviet.Server;
using Iconlook.Object;
using Iconlook.Server;
using NServiceBus;
using Serilog;
using ServiceStack;
using JsonHttpClient = Iconlook.Client.JsonHttpClient;

namespace Iconlook.Service.Mon.Works
{
    public class UpdatePeersWork : WorkBase
    {
        public static long StartCount;

        public override async Task StartAsync()
        {
            StartCount++;
            using (var rolex = new Rolex())
            {
                Log.Debug("{Work} started", nameof(UpdatePeersWork));
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
                                    var client = new JsonHttpClient(1.75);
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
                                if (StartCount % 10 == 0)
                                {
                                    redis.StoreAll(peers);
                                }
                                await Endpoint.Instance().Publish(new PeersUpdatedEvent
                                {
                                    Busy = peers.Where(x => x != null && x.State == "BlockGenerate").ToList()
                                }).ConfigureAwait(false);
                                await Channel.Instance().Publish(new PeersUpdatedSignal
                                {
                                    Idle = peers.Where(x => x != null && x.State == "Vote").ToList(),
                                    Sync = peers.Where(x => x != null && x.State == "BlockSync").ToList(),
                                    Busy = peers.Where(x => x != null && x.State == "BlockGenerate").ToList(),
                                    Down = peers.Where(x => x != null && x.State == "LeaderComplain").ToList()
                                }).ConfigureAwait(false);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "{Work} failed to run. {Message}", nameof(UpdatePeersWork), exception.Message);
                }
                Log.Debug("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdatePeersWork), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}