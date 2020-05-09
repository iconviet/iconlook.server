using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Iconlook.Client;
using Iconlook.Client.Megaloop;
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
                    var service = new MegaloopClient(Endpoints.TESTNET, 2);
                    var players = await service.GetPlayers();
                    var last_player = await service.GetLastPlayer();
                    var last_winner = await service.GetLastWinner();
                    var jackpot_size = await service.GetJackpotSize();
                    var megaloop = new MegaloopResponse
                    {
                        PlayerCount = players.Count(),
                        JackpotSize = jackpot_size.ToIcx(),
                        LastPlayer = new MegaloopPlayerResponse
                        {
                            Address = last_player.Split(':')[0],
                            Block = long.Parse(last_player.Split(':')[2]),
                            Deposit = BigInteger.Parse(last_player.Split(':')[1]).ToIcx()
                            
                        },
                        JackpotSizeUsd = jackpot_size.ToIcx() * UpdateChainWorker.LastIcxPrice,
                        Players = players.Select(address => new MegaloopPlayerResponse
                        {
                            Address = address.ToString().Split(":")[0],
                            Block =  long.Parse(address.ToString().Split(":")[2]),
                            Deposit = BigInteger.Parse(address.ToString().Split(":")[1]).ToIcx(),
                            Chance = BigInteger.Parse(address.ToString().Split(":")[1]).ToIcx() / jackpot_size.ToIcx()
                        }).ToList(),
                        LastWinner = new MegaloopWinnerResponse
                        {
                            Address = last_winner.Split(':')[0],
                            Deposit = BigInteger.Parse(last_winner.Split(':')[1]).ToIcx(),
                            Jackpot = BigInteger.Parse(last_winner.Split(':')[2]).ToIcx(),
                            Subsidy = BigInteger.Parse(last_winner.Split(':')[3]).ToIcx(),
                            JackpotUsd = BigInteger.Parse(last_winner.Split(':')[2]).ToIcx() * UpdateChainWorker.LastIcxPrice,
                        }
                    };
                    await Channel.Instance().Publish(new MegaloopUpdatedSignal { Megaloop = megaloop }).ConfigureAwait(false);
                    await Endpoint.Instance().Publish(new MegaloopUpdatedEvent { Megaloop = megaloop }).ConfigureAwait(false);
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