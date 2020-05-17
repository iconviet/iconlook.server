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
using ServiceStack;

namespace Iconlook.Service.Job.Workers
{
    public class UpdateMegaloopWorker : WorkerBase
    {
        public static MegaloopPlayerResponse DiffPlayer;

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
                    var jackpot_subsidy = await client.GetJackpotSubsidy();
                    var response = new MegaloopResponse
                    {
                        PlayerCount = players.Count(),
                        JackpotSize = jackpot_size.ToIcx(),
                        JackpotSubsidy = jackpot_subsidy.ToIcx()
                    }.ThenDo(x =>
                    {
                        x.TotalJackpotSize = x.JackpotSize + x.JackpotSubsidy;
                        x.TotalJackpotSizeUsd = x.TotalJackpotSize * last_icx_price;
                    });
                    if (last_player.HasValue())
                    {
                        response.LastPlayer = new MegaloopPlayerResponse
                        {
                            Address = last_player.Split(':')[0],
                            Deposit = last_player.Split(':')[1].ToIcx(),
                            Block = long.Parse(last_player.Split(':')[2])
                        };
                    }
                    if (players.Any())
                    {
                        response.Players = players.Select(player => new MegaloopPlayerResponse
                        {
                            Address = player.ToString().Split(":")[0],                            
                            Deposit = player.ToString().Split(":")[1].ToIcx(),
                            Block = long.Parse(player.ToString().Split(":")[2]),
                            Chance = player.ToString().Split(":")[1].ToIcx() / jackpot_size.ToIcx()
                        }).OrderByDescending(x => x.Block).Take(20).ToList();
                    }
                    if (last_winner.HasValue())
                    {
                        response.LastWinner = new MegaloopWinnerResponse
                        {
                            Address = last_winner.Split(':')[1],
                            Deposit = last_winner.Split(':')[2].ToIcx(),
                            Jackpot = last_winner.Split(':')[3].ToIcx(),
                            Subsidy = last_winner.Split(':')[4].ToIcx(),
                            JackpotUsd = last_winner.Split(':')[3].ToIcx() * last_icx_price,
                            Chance = last_winner.Split(':')[3].ToIcx() / last_winner.Split(':')[2].ToIcx()
                        };
                    }
                    if (winners.Any())
                    {
                        response.Winners = winners.Select(winner => new MegaloopWinnerResponse
                        {
                            Address = winner.ToString().Split(':')[1],                            
                            Deposit = winner.ToString().Split(':')[2].ToIcx(),
                            Jackpot = winner.ToString().Split(':')[3].ToIcx(),
                            Subsidy = winner.ToString().Split(':')[4].ToIcx(),
                            Block = long.Parse(winner.ToString().Split(':')[0]),
                            JackpotUsd = winner.ToString().Split(':')[3].ToIcx() * last_icx_price,
                            Chance = winner.ToString().Split(':')[3].ToIcx() / last_winner.Split(':')[2].ToIcx()
                        }).OrderByDescending(x => x.Block).Take(20).ToList();
                    }
                    await Endpoint.Instance().Publish(new MegaloopUpdatedEvent
                    {
                        Megaloop = response
                    }).ConfigureAwait(false);
                    await Channel.Instance().Publish(new MegaloopUpdatedSignal
                    {
                        LastPlayer = response.LastPlayer,
                        LastWinner = response.LastWinner,
                        PlayerCount = response.PlayerCount,
                        JackpotSize = response.JackpotSize,                        
                        JackpotSubsidy = response.JackpotSubsidy,
                        TotalJackpotSize = response.TotalJackpotSize,
                        TotalJackpotSizeUsd = response.TotalJackpotSizeUsd
                    }.ThenDo(signal => {
                        if (DiffPlayer?.Address != response.LastPlayer?.Address)
                        {
                            signal.DiffPlayer = response.LastPlayer;
                            DiffPlayer = response.LastPlayer;
                        }
                    })).ConfigureAwait(false);
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