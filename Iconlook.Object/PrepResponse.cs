using System;
using Agiper.Object;

namespace Iconlook.Object
{
    public class PrepResponse : ResponseBase<PrepResponse>
    {
        public int Id { get; set; }
        public int Votes { get; set; }
        public int Score { get; set; }
        public int Voters { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public bool Direction { get; set; }
        public string Location { get; set; }
        public DateTime Joined { get; set; }
        public DateTime LastSeen { get; set; }
        public int ProducedBlocks { get; set; }
        public int RejectedBlocks { get; set; }
        public DateTime Timestamp { get; set; }
        public double UptimePercentage { get; set; }
        public double SupplyPercentage { get; set; }
    }
}