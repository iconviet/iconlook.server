using System;
using System.Linq;
using System.Threading.Tasks;
using Iconlook.Client;
using Iconlook.Client.Service;
using Iconlook.Message;
using Iconlook.Object;
using Iconlook.Server;
using Iconviet;
using Iconviet.Server;
using NServiceBus;
using Serilog;

namespace Iconlook.Service.Job.Workers
{
    public class UpdateMegaloopWorker : WorkerBase
    {
        public override async Task StartAsync()
        {
            using (var rolex = new Rolex())
            {
                Log.Debug("{Work} started", nameof(UpdateMegaloopWorker));
                try
                {
                    var client = new MegaloopScoreClient(2);
                    var players = await client.GetPlayers();
                    var winners = await client.GetWinners();
                    var last_player = await client.GetLastPlayer();
                    var last_winner = await client.GetLastWinner();
                    var jackpot_size = await client.GetJackpotSize();
                    var last_icx_price = UpdateChainWorker.LastIcxPrice;
                    var current_subsidy = await client.GetCurrentSubsidy();
                    var megaloop = new MegaloopResponse
                    {
                        PlayerCount = players.Count(),
                        CurrentSubsidy = current_subsidy.ToLoop().ToIcx(),
                        JackpotSizeUsd = jackpot_size.ToIcx() * last_icx_price,
                        JackpotSize = jackpot_size.ToIcx() + current_subsidy.ToLoop().ToIcx()
                    };
                    if (last_player.HasValue())
                    {
                        megaloop.LastPlayer = new MegaloopPlayerResponse
                        {
                            Address = last_player.Split(':')[0],
                            Block = long.Parse(last_player.Split(':')[2]),
                            Deposit = last_player.Split(':')[1].ToLoop().ToIcx()
                        };
                    }
                    if (players.Any())
                    {
                        megaloop.Players = players.Select(player => new MegaloopPlayerResponse
                        {
                            Address = player.ToString().Split(":")[0],
                            Block = long.Parse(player.ToString().Split(":")[2]),
                            Deposit = player.ToString().Split(":")[1].ToLoop().ToIcx(),
                            Chance = player.ToString().Split(":")[1].ToLoop().ToIcx() / jackpot_size.ToIcx()
                        }).OrderByDescending(x => x.Block).Take(20).ToList();
                    }
                    if (last_winner.HasValue())
                    {
                        megaloop.LastWinner = new MegaloopWinnerResponse
                        {
                            Address = last_winner.Split(':')[1],
                            Deposit = last_winner.Split(':')[2].ToLoop().ToIcx(),
                            Jackpot = last_winner.Split(':')[3].ToLoop().ToIcx(),
                            Subsidy = last_winner.Split(':')[4].ToLoop().ToIcx(),
                            JackpotUsd = last_winner.Split(':')[3].ToLoop().ToIcx() * last_icx_price,
                            Chance = last_winner.Split(':')[3].ToLoop().ToIcx() / last_winner.Split(':')[2].ToLoop().ToIcx()
                        };
                    }
                    if (winners.Any())
                    {
                        megaloop.Winners = winners.Select(winner => new MegaloopWinnerResponse
                        {
                            Address = winner.ToString().Split(':')[1],
                            Block = long.Parse(winner.ToString().Split(':')[0]),
                            Deposit = winner.ToString().Split(':')[2].ToLoop().ToIcx(),
                            Jackpot = winner.ToString().Split(':')[3].ToLoop().ToIcx(),
                            Subsidy = winner.ToString().Split(':')[4].ToLoop().ToIcx(),
                            JackpotUsd = winner.ToString().Split(':')[3].ToLoop().ToIcx() * last_icx_price,
                            Chance = winner.ToString().Split(':')[3].ToLoop().ToIcx() / last_winner.Split(':')[2].ToLoop().ToIcx()
                        }).OrderByDescending(x => x.Block).Take(20).ToList();
                    }
                    await Endpoint.Instance().Publish(new MegaloopUpdatedEvent
                    {
                        Megaloop = megaloop
                    }).ConfigureAwait(false);
                    await Channel.Instance().Publish(new MegaloopUpdatedSignal
                    {
                        LastPlayer = megaloop.LastPlayer,
                        LastWinner = megaloop.LastWinner,
                        PlayerCount = megaloop.PlayerCount,
                        JackpotSize = megaloop.JackpotSize,
                        JackpotSizeUsd = megaloop.JackpotSizeUsd
                    }).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    if (!(exception is TaskCanceledException))
                    {
                        Log.Error(exception, "{Work} failed to run. {Message}", nameof(UpdateMegaloopWorker), exception.Message);
                    }
                }
                Log.Debug("{Work} stopped ({Elapsed:N0}ms)", nameof(UpdateMegaloopWorker), rolex.Elapsed.TotalMilliseconds);
            }
        }
    }
}