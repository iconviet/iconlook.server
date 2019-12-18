using System;
using Agiper.Object;

namespace Iconlook.Object
{
    public class PRepResponse : ResponseBase<PRepResponse>
    {
        public string Id { get; set; }
        public int Votes { get; set; }
        public int Score { get; set; }
        public int Voters { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
        public bool Testnet { get; set; }
        public string Goals { get; set; }
        public string Entity { get; set; }
        public string Address { get; set; }
        public bool Direction { get; set; }
        public double Balance { get; set; }
        public string Hosting { get; set; }
        public string Regions { get; set; }
        public string Location { get; set; }
        public string Identity { get; set; }
        public int MissedBlocks { get; set; }
        public string P2PEndpoint { get; set; }
        public int ProducedBlocks { get; set; }
        public DateTimeOffset Joined { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public double DelegatedPercentage { get; set; }
        public double ProductivityPercentage { get; set; }
    }
}