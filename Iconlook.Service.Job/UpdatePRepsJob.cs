using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client.Service;
using Iconlook.Object;

namespace Iconlook.Service.Job
{
    public class UpdatePRepsJob : JobBase
    {
        private static readonly IconServiceClient Icon = new IconServiceClient();
        private static readonly Dictionary<string, PRep> PReps = new Dictionary<string, PRep>();

        public override async Task RunAsync()
        {
            if (!PReps.Any())
            {
                foreach (var prep in await Icon.GetPReps())
                {
                    PReps.TryAdd(prep.GetAddress().ToString(), prep);
                }
            }
        }

        public Entity.PRep GetNewPRep()
        {
            return new Entity.PRep
            {
                Name = "",
                Location = "",
                IdExternal = "",
                Joined = DateTime.UtcNow,
                State = PRepState.Enabled,
                LastSeen = DateTime.UtcNow,
                Size = new Random().Next(1, 10),
                Position = new Random().Next(1, 66),
                Score = new Random().Next(-100, 100),
                Voters = new Random().Next(100, 1000),
                Votes = new Random().Next(1000000, 10000000),
                Direction = new Random().NextDouble() >= 0.5,
                Balance = new Random().Next(100000, 10000000),
                MissedBlocks = new Random().Next(100, 1000),
                ProducedBlocks = new Random().Next(100000, 1000000),
                Address = "hxd2d001c3938c7f6d31bc76b1cda922a64c51c8bf",
                Testnet = new[] { true, false }[new Random().Next(0, 1)],
                ProductivityPercentage = new Random().NextDouble() * (0.1 - -0.1) + -0.1,
                Entity = new[] { "Company", "Group", "Individual" }[new Random().Next(0, 3)],
                DelegatedPercentage = (double) new Random().Next(1000000, 10000000) / 490000000,
                Identity = new[] { "Verified", "Unknown", "Anonymous" }[new Random().Next(0, 3)],
                Regions = new[] { "Asia", "Europe", "US", "Australia" }[new Random().Next(0, 4)],
                Goals = new[] { "Development", "Awareness", "Expansion" }[new Random().Next(0, 3)],
                Hosting = new[] { "Azure", "Amazon", "Google", "Bare Metal" }[new Random().Next(0, 4)]
            };
        }
    }
}